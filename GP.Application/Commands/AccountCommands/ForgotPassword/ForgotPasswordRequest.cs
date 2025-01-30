using GP.Core.Constants;
using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.AccountCommands.ForgotPassword
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = ResourceKey.Required)]
        public string EmailOrUsername { get; set; }
        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(RegexConstants.PasswordRegex,
            ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
