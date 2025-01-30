using GP.Core.Constants;
using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.UserCommands.ResetPassword
{
    public class ResetPasswordRequest
    {
        public string ConfirmationHash { get; set; }
        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string NewPassword { get; set; }
    }
}
