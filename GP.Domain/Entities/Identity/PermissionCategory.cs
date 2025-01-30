using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations;

namespace GP.Domain.Entities.Identity
{
    public class PermissionCategory : Entity
    {
        [Key]
        [Required]
        [StringLength(32)]
        public string? Label { get; set; }
        [StringLength(128)]
        public string? VisibleLabel { get; set; }
        [StringLength(256)]
        public string? Description { get; set; }

        public ICollection<PermissionCategoryPermission> PossiblePermissions { get; set; } = [];
    }
}
