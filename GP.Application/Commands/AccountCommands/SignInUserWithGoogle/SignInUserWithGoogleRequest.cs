using System.ComponentModel.DataAnnotations;
using GP.Core.Resources;

namespace GP.Application.Commands.AccountCommands.SignInUserWithGoogle
{
    public class SignInUserWithGoogleRequest
    {
        /// <summary>
        /// User email or Username
        /// </summary>
        [Required(ErrorMessage = ResourceKey.Required)]
        public string Email { get; set; }
        /// <summary>
        /// User email or Username
        /// </summary>
        [Required(ErrorMessage = ResourceKey.Required)]
        public string FullName { get; set; }
    }
}
