using AutoMapper;
using GP.Application.Commands.UserCommands.LockUser;
using GP.Application.Queries.UserQueries;
using GP.Core.Enums.Enitity;
using GP.Core.Extensions;
using GP.Core.Models;
using GP.Core.Resources;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.Infrastructure.Services.SmsConfirmationService;
using GP.Infrastructure.Services.SmsService;
using MediatR;
using Microsoft.Extensions.Options;

namespace GP.Application.Commands.SmsConfirmationCodeCommands.SendConfirmationCode
{
    public class SendConfirmationCodeCommandHandler : ICommandHandler<SendConfirmationCodeCommand, SendConfirmationCodeResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly SmsConfirmationCodeService _smsConfirmationCodeService;
        private readonly SmsSenderService _smsSenderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthSettings _authSettings;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ExceptionService _exceptionService;

        public SendConfirmationCodeCommandHandler(IUserRepository userRepository,
            SmsConfirmationCodeService smsConfirmationCodeService,
            SmsSenderService smsSenderService, IUnitOfWork unitOfWork,
            IOptions<AuthSettings> authSettings, IMediator mediator, ExceptionService exceptionService, IMapper mapper)
        {
            _userRepository = userRepository;
            _smsConfirmationCodeService = smsConfirmationCodeService;
            _smsSenderService = smsSenderService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _exceptionService = exceptionService;
            _mapper = mapper;
            _authSettings = authSettings.Value;
        }

        public async Task<SendConfirmationCodeResponse> Handle(SendConfirmationCodeCommand command, CancellationToken cancellationToken)
        {
            var smsRequestType = command.SmsRequestTypeEnum;
            var emailOrUsername = command.Request.EmailOrUsername;
            var isEmail = emailOrUsername.IsEmail();

            User user;
            if (isEmail)
                user = await _userRepository.GetUserByEmailAsync(emailOrUsername)
                    .ConfigureAwait(false);
            else
                user = await _userRepository.GetUserByNameAsync(emailOrUsername)
                    .ConfigureAwait(false);

            if (user == null)
                throw _exceptionService.RecordNotFoundException(ResourceKey.UserNotFoundContactToAdmin);

            var result = _mapper.Map<User, UserInfoResponse>(user);
            if (result.IsLocked) throw _exceptionService.LockedUserException();

            if (smsRequestType == SmsRequestTypeEnum.ForgotPassword && user.UserType == UserTypeEnum.LdapUser)
                throw _exceptionService.CorporateUserSignInException();

            var overOtpCount = user.OtpSentCount == (_authSettings.MaxFailedAccessAttempts - 1);

            if (overOtpCount)
            {
                await _mediator.Send(new LockUserCommand(new LockUserRequest()
                {
                    UserId = user.Id,
                    ExpireDate = DateTime.Now.AddMinutes(_authSettings.OtpExpiry)
                }), cancellationToken);
                throw _exceptionService.LockedUserException();
            }

            var code = await _smsConfirmationCodeService.GetConfirmationCode(user.Id, user.PhoneNumber, smsRequestType);
            await _smsSenderService.SendConfirmationSmsAsync(user.Id, user.PhoneNumber, code, _authSettings.OtpExpiry);

            _userRepository.IncreaseOtpSentCount(user);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return new SendConfirmationCodeResponse()
            {
                RequiredConfirm = true,
                Message = "Confirmation code is sent to " + user.PhoneNumber.PhoneNumberEncrypted()
            };
        }
    }
}
