using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Domain.Entities.Identity
{
    public class PermissionCategoryPermission : Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Category")]
        public string? CategoryId { get; set; }
        [ForeignKey("Permission")]
        public string? PermissionId { get; set; }
        public PermissionCategory? Category { get; set; }
        public Permission? Permission { get; set; }
        public ICollection<RolePermissionCategory> Roles { get; set; } = [];
    }
}
