using GP.Core.Enums.Enitity;
using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations;

namespace GP.Domain.Entities.Identity
{
    public class UserSmsConfirmationRequest : Entity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
        public string IpAddress { get; set; }
        public string Phone { get; set; }
        public SmsConfirmationStatusEnum ConfirmationStatus { get; set; }
        public SmsRequestTypeEnum SmsRequestTypeEnum { get; set; } = SmsRequestTypeEnum.Auth;
    }
}
