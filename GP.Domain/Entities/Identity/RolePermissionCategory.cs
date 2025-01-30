using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Domain.Entities.Identity
{
    public class RolePermissionCategory : Entity
    {
        [ForeignKey("PermissionCategoryPermission")]
        public int PermissionCategoryPermissionId { get; set; }
        public PermissionCategoryPermission PermissionCategoryPermission { get; set; }

        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
