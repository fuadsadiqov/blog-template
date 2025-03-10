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
        private readonly SignInService _signInService;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;

        public SignInUserCommandHandler(SignInService signInService, IUserRepository userRepository, ExceptionService exceptionService)
        {
            _signInService = signInService;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
        }
        
        public async Task<SignInUserResponse> Handle(SignInUserCommand command, CancellationToken cancellationToken)
        {
            var emailOrUsername = command.Request.EmailOrUsername;
            var password = command.Request.Password;
            var isEmail = emailOrUsername.IsEmail();

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

            return new SignInUserResponse()
            {
                IsSigned = isSignableUser
            };
        }
        private async Task<UserSignInResult> SignInUser(User user, string password)
        {
            UserSignInResult signInResult;

            signInResult = await _signInService.LocalSignInAsync(user, password);

            if (signInResult.IsLockOut)
                throw _exceptionService.LockedUserException();

            return signInResult;
        }

    }
}







