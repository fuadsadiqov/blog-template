using GP.Core.Constants;
using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.UserCommands.ChangeUserPassword
{
    public class ChangeUserPasswordRequest
    {
        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(RegexConstants.PasswordRegex,
            ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(RegexConstants.PasswordRegex,
            ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string Password { get; set; }

        [Required(ErrorMessage = ResourceKey.Required)]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
