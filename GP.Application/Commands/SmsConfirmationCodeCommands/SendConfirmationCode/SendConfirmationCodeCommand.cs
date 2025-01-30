using GP.Core.Enums.Enitity;
using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.SmsConfirmationCodeCommands.SendConfirmationCode
{
    public class SendConfirmationCodeCommand : CommandBase<SendConfirmationCodeResponse>
    {
        public SendConfirmationCodeCommand(SendConfirmationCodeRequest request, SmsRequestTypeEnum smsRequestTypeEnum)
        {
            Request = request;
            SmsRequestTypeEnum = smsRequestTypeEnum;
        }
        public SmsRequestTypeEnum SmsRequestTypeEnum { get; set; }
        public SendConfirmationCodeRequest Request { get; set; }
    }
}
