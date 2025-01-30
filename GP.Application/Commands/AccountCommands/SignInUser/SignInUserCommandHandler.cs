using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using GP.Core.Extensions;
using GP.Core.Models;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.Application.Commands.SmsConfirmationCodeCommands.SendConfirmationCode;
using GP.Application.Commands.SmsConfirmationCodeCommands.ValidateConfirmationCode;
using GP.Core.Enums.Enitity;
using GP.Application.Queries.AppOptionQueries.GetUserAppOption;
using GP.Application.Queries.AppOptionQueries;

namespace GP.Application.Commands.AccountCommands.SignInUser
{
    public class SignInUserCommandHandler : ICommandHandler<SignInUserCommand, SignInUserResponse>
    {
        private readonly AuthService _authService;
        private readonly SignInService _signInService;
        private readonly IUserRepository _userRepository;
        private readonly AccessLimitService _accessLimitService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ExceptionService _exceptionService;
        private readonly TokenService _tokenService;

        public SignInUserCommandHandler(AuthService authService, SignInService signInService, IUserRepository userRepository, IMediator mediator, IHttpContextAccessor httpContextAccessor, AccessLimitService accessLimitService, ExceptionService exceptionService, TokenService tokenService)
        {
            _authService = authService;
            _signInService = signInService;
            _userRepository = userRepository;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _accessLimitService = accessLimitService;
            _exceptionService = exceptionService;
            _tokenService = tokenService;
        }
        
        public async Task<SignInUserResponse> Handle(SignInUserCommand command, CancellationToken cancellationToken)
        {
            const SmsRequestTypeEnum smsRequestType = SmsRequestTypeEnum.Auth;
            var emailOrUsername = command.Request.EmailOrUsername;
            var password = command.Request.Password;
            var isEmail = emailOrUsername.IsEmail();
            //var isPinRequired = false;

            User user;
            if (isEmail)
                user = await _userRepository.GetUserByEmailAsync(emailOrUsername)
                    .ConfigureAwait(false);
            else
                user = await _userRepository.GetUserByNameAsync(emailOrUsername)
                    .ConfigureAwait(false);

            if (user == null)
                throw _exceptionService.InvalidSignInException();

            var isActiveUser = user.Status == RecordStatusEnum.Active;
            if (!isActiveUser)
                throw _exceptionService.AccessDeniedException();

            var isSignableUser = (await SignInUser(user, password)).Succeeded;
            if (!isSignableUser)
                throw _exceptionService.InvalidSignInException();


            _accessLimitService.CheckCanAccessUserOutOfDomain(user);

            var appOption = (await _mediator.Send(new GetUserAppOptionQuery(new GetUserAppOptionRequest()
            {
                UserId = user.Id
            }), cancellationToken)).Response;

            var canAccessWithoutOtp = CanAccessWithoutOtp(appOption, user, cancellationToken);

            if (canAccessWithoutOtp) return await GetSigninTokenAsync(user, appOption, command.Request.RememberMe, cancellationToken);

            //if code exist then check code
            var isOtpCodeExist = !string.IsNullOrEmpty(command.Request.Code);
            if (isOtpCodeExist)
            {
                await _mediator.Send(new ValidateConfirmationCodeCommand(new ValidateConfirmationCodeRequest()
                {
                    EmailOrUsername = command.Request.EmailOrUsername,
                    Code = command.Request.Code,
                }, new ValidationOptions()
                {
                    ResendSms = true,
                    NeedApprove = true,
                    SmsRequestTypeEnum = SmsRequestTypeEnum.Auth
                }), cancellationToken);


                return await GetSigninTokenAsync(user, appOption, command.Request.RememberMe, cancellationToken);
            }

            var response = await _mediator.Send(new SendConfirmationCodeCommand(new SendConfirmationCodeRequest()
            {
                EmailOrUsername = command.Request.EmailOrUsername
            }, smsRequestType), cancellationToken);

            return new SignInUserResponse()
            {
                RequiredConfirm = response.RequiredConfirm,
                IsPinRequired = appOption?.IsPinRequired ?? true,
                Message = response.Message
            };
        }
        #region Private methods

        private async Task<SignInUserResponse> GetSigninTokenAsync(User user, AppOptionResponse appOption, bool rememberMe, CancellationToken cancellationToken)
        {
            var applicationHeader = _httpContextAccessor.HttpContext.GeApplicationHeaderValue();
            var domain = _httpContextAccessor.HttpContext.GetDomainUrl();

            var applicationClaim = new Claim(CustomClaimTypes.Application, applicationHeader);
            var domainClaim = new Claim(CustomClaimTypes.Domain, domain);
            //var rememberMeForClaim = rememberMe && applicationHeader.ToLower() == ApplicationClaimValues.Mobile.ToLower();
            var rememberMeForClaim = rememberMe;
            var rememberMeClaim = new Claim(CustomClaimTypes.RememberMe, rememberMeForClaim.ToString());
            var canAccessOutOfDomainClaim = new Claim(CustomClaimTypes.CanAccessOutOfDomain, user.CanAccessOutOfDomain.ToString());

            var claims = new List<Claim>()
            {
                applicationClaim,
                domainClaim,
                rememberMeClaim,
                canAccessOutOfDomainClaim
            };

            var accessToken = _tokenService.GenerateToken(user, claims.ToArray());
            await _tokenService.RemoveOldRefreshTokensAsync(user.Id, domainClaim.Value, applicationClaim.Value, string.Empty, removeAll: true);
            var refreshToken = await _tokenService.AddRefreshTokenAsync(user.Id, new TokenClaim(claims));

            return new SignInUserResponse()
            {
                IsPinRequired = appOption?.IsPinRequired ?? true,
                RequiredConfirm = false,
                IsSigned = true,
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        private bool CanAccessWithoutOtp(AppOptionResponse appOption, User user, CancellationToken cancellationToken = default)
        {
            var isLocalIpAddress = _authService.IsUserIpAddressLocal();//true


            var isLdapUser = user.UserType == UserTypeEnum.LdapUser;//true

            var isOtpRequired = appOption?.IsOtpRequired ?? true;
            var isDevelopment = EnvironmentExtension.IsDevelopment;

            if (isLdapUser && isLocalIpAddress)
                return true;


            if (isDevelopment || !isOtpRequired)
                return true;

            return false;
        }

        private async Task<UserSignInResult> SignInUser(User user, string password)
        {
            UserSignInResult signInResult;

            /*if (user.UserType == UserTypeEnum.LdapUser)
            {
                signInResult = await _signInService.LdapSignInAsync(user.UserName, password);
                if (!signInResult.Succeeded)
                    signInResult = await _signInService.LocalSignInAsync(user, password);
            }
            else
            {
                signInResult = await _signInService.LocalSignInAsync(user, password);
            }*/

            signInResult = await _signInService.LocalSignInAsync(user, password);

            if (signInResult.IsLockOut)
                throw _exceptionService.LockedUserException();

            return signInResult;
        }
        #endregion
    }
}







