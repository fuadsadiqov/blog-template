using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.SmsConfirmationCodeCommands.SendConfirmationCode
{
    public class SendConfirmationCodeRequest
    {
        [Required(ErrorMessage = ResourceKey.Required)]
        public string EmailOrUsername { get; set; }
    }
}
