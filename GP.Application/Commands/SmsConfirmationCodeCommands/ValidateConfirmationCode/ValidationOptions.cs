using GP.Core.Enums.Enitity;

namespace GP.Application.Commands.SmsConfirmationCodeCommands.ValidateConfirmationCode
{
    public class ValidationOptions
    {
        public SmsRequestTypeEnum SmsRequestTypeEnum { get; set; }
        public bool ResendSms { get; set; }
        public bool NeedApprove { get; set; }
    }
}
