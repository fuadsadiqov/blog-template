using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations;

namespace GP.Domain.Entities.Identity
{
    public class PasswordResetRequest : Entity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
