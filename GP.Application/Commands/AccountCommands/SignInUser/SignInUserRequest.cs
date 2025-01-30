using System.ComponentModel.DataAnnotations;
using GP.Core.Resources;

namespace GP.Application.Commands.AccountCommands.SignInUser
{
    public class SignInUserRequest
    {
        /// <summary>
        /// User email or Username
        /// </summary>
        [Required(ErrorMessage = ResourceKey.Required)]
        public string EmailOrUsername { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Password is not defined")]
        public string Password { get; set; }
        /// <summary>
        /// Otp code when response has RequiredConfirm field is true
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// When true you get 90 days jwt.Default value is false
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }
}
