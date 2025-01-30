using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.SmsConfirmationCodeCommands.ValidateConfirmationCode
{
    public class ValidateConfirmationCodeCommand : CommandBase<ValidateConfirmationCodeResponse>
    {
        public ValidateConfirmationCodeCommand(ValidateConfirmationCodeRequest request, ValidationOptions validationOptions)
        {
            ValidationOptions = validationOptions;
            Request = request;
        }
        public ValidationOptions ValidationOptions { get; set; }
        public ValidateConfirmationCodeRequest Request { get; set; }
    }
}
