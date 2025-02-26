using System.ComponentModel.DataAnnotations;
using GP.Core.Constants;
using GP.Core.Enums.Enitity;
using GP.Core.Resources;

namespace GP.Application.Commands.UserCommands.CreateUserWithGoogle
{
    public class CreateUserWithGoogleRequest
    {

        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(
            RegexConstants.EmailRegex,
            ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string Email { get; set; }

        [Required(ErrorMessage = ResourceKey.Required)]
        [StringLength(StringLengthConstants.Small)]
        public string FullNameEn { get; set; }
        [Required(ErrorMessage = ResourceKey.Required)]
        [StringLength(StringLengthConstants.Small)]
        public string FullNameAz { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> DirectivePermissions { get; set; } = new List<string>();
        public bool CanAccessOutOfDomain { get; set; }
    }
}
