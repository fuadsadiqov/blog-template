using GP.Core.Constants;
using GP.Core.Enums.Enitity;
using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.UserCommands.UpdateUser
{
    public class UpdateUserRequest
    {
        public string Id { get; set; }
        [RegularExpression(RegexConstants.LocalPhoneNumberRegex, ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string PhoneNumber { get; set; }
        public UserTypeEnum UserType { get; set; } = UserTypeEnum.LdapUser;
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
        public List<string> Roles { get; set; }
        public List<string> DirectivePermissions { get; set; }
        public bool CanAccessOutOfDomain { get; set; }
    }
}
