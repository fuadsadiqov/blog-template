using GP.Core.Configurations.Entity;
using GP.Core.Enums.Enitity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Domain.Entities.Identity
{
    public class Role : IdentityRole<string>, IEntity
    {
        public ICollection<UserRole> Users { get; set; } = [];
        public ICollection<RolePermissionCategory> PermissionCategory { get; set; } = [];
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? Uuid { get; set; }
        public RecordStatusEnum Status { get; set; }
        public User? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public string? CreatedById { get; set; }
        public User? ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public string? ModifiedById { get; set; }
    }
}
