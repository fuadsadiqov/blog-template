using GP.Application.Commands.SmsConfirmationCodeCommands.ValidateConfirmationCode;
using GP.Application.Commands.UserCommands.ChangeUserPassword;
using GP.Core.Enums.Enitity;
using GP.Core.Extensions;
using GP.Core.Resources;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;

namespace GP.Application.Commands.AccountCommands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, ForgotPasswordResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IMediator mediator, ExceptionService exceptionService)
        {
            _userRepository = userRepository;
            _mediator = mediator;
            _exceptionService = exceptionService;
        }

        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
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
                throw _exceptionService.RecordNotFoundException(ResourceKey.UserNotFoundContactToAdmin);

            if (string.IsNullOrEmpty(command.Request.Code))
                throw _exceptionService.InvalidSmsConfirmationCodeException();

            await _mediator.Send(new ValidateConfirmationCodeCommand(new ValidateConfirmationCodeRequest()
            {
                EmailOrUsername = command.Request.EmailOrUsername,
                Code = command.Request.Code,
            }, new ValidationOptions()
            {
                ResendSms = false,
                NeedApprove = true,
                SmsRequestTypeEnum = SmsRequestTypeEnum.ForgotPassword
            }), cancellationToken);

            var requestChangePassword = new ChangeUserPasswordRequest()
            {
                Password = password
            };

            await _mediator.Send(new ChangeUserPasswordCommand(user.Id, true, requestChangePassword)
            {
            }, cancellationToken);

            return new ForgotPasswordResponse();
        }
    }
}
