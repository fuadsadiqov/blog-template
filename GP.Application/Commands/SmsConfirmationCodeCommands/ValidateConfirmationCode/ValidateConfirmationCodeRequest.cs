using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.SmsConfirmationCodeCommands.ValidateConfirmationCode
{
    public class ValidateConfirmationCodeRequest
    {
        [Required(ErrorMessage = ResourceKey.Required)]
        public string EmailOrUsername { get; set; }
        public string Code { get; set; }
    }
}
