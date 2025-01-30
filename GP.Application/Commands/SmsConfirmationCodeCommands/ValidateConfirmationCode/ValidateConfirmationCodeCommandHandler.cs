using GP.Application.Commands.SmsConfirmationCodeCommands.SendConfirmationCode;
using GP.Core.Enums.Enitity;
using GP.Core.Extensions;
using GP.Core.Resources;
using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services.SmsConfirmationService;
using GP.Infrastructure.Services;
using MediatR;

namespace GP.Application.Commands.SmsConfirmationCodeCommands.ValidateConfirmationCode
{
    public class ValidateConfirmationCodeCommandHandler : ICommandHandler<ValidateConfirmationCodeCommand, ValidateConfirmationCodeResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly SmsConfirmationCodeService _smsConfirmationCodeService;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ExceptionService _exceptionService;

        public ValidateConfirmationCodeCommandHandler(IUserRepository userRepository,
          SmsConfirmationCodeService smsConfirmationCodeService,
          IMediator mediator, IUnitOfWork unitOfWork, ExceptionService exceptionService)
        {
            _userRepository = userRepository;
            _smsConfirmationCodeService = smsConfirmationCodeService;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _exceptionService = exceptionService;
        }

        public async Task<ValidateConfirmationCodeResponse> Handle(ValidateConfirmationCodeCommand command,
            CancellationToken cancellationToken)
        {
            var smsRequestType = command.ValidationOptions.SmsRequestTypeEnum;
            var needApprove = command.ValidationOptions.NeedApprove;
            var resendSms = command.ValidationOptions.ResendSms;
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

            if (smsRequestType == SmsRequestTypeEnum.ForgotPassword && user.UserType == UserTypeEnum.LdapUser)
                throw _exceptionService.CorporateUserSignInException();

            if (string.IsNullOrEmpty(command.Request.Code)) throw _exceptionService.InvalidSmsConfirmationCodeException();

            //if code is not valid then send sms again and throw exception
            var isValidCode =
                await _smsConfirmationCodeService.
                    IsValidConfirmationCodeAsync(user.Id,
                        user.PhoneNumber,
                        command.Request.Code,
                        smsRequestType, needApprove);

            if (!isValidCode)
            {
                if (resendSms)
                {
                    //send sms again
                    await _mediator.Send(new SendConfirmationCodeCommand(new SendConfirmationCodeRequest()
                    {
                        EmailOrUsername = command.Request.EmailOrUsername
                    }, smsRequestType), cancellationToken);
                    throw _exceptionService.InvalidSmsConfirmationCodeAndResendException();
                }
                throw _exceptionService.InvalidSmsConfirmationCodeException();
            }

            if (user.OtpSentCount != 0)
            {
                _userRepository.ResetOtpSentCount(user);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            return new ValidateConfirmationCodeResponse();
        }
    }
}
