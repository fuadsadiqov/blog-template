using System.ComponentModel.DataAnnotations;
using GP.Core.Constants;
using GP.Core.Enums.Enitity;
using GP.Core.Resources;

namespace GP.Application.Commands.UserCommands.CreateUser
{
    public class CreateUserRequest
    {

        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(
            RegexConstants.EmailRegex,
            ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string Email { get; set; }

        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(RegexConstants.UserNameRegex, ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string UserName { get; set; }

        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [RegularExpression(RegexConstants.LocalPhoneNumberRegex, ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string PhoneNumber { get; set; }
        public UserTypeEnum UserType { get; set; } = UserTypeEnum.LdapUser;
        [Required(ErrorMessage = ResourceKey.Required)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [StringLength(StringLengthConstants.Medium)]
        public string PositionAz { get; set; }
        [StringLength(StringLengthConstants.Medium)]
        public string PositionEn { get; set; }
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
