using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GP.Domain.Entities.Identity
{
    public class UserPermission : Entity
    {
        [ForeignKey("Role")]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        public User User { get; set; }
        public Permission Permission { get; set; }
    }
}
