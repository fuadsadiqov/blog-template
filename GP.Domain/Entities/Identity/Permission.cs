using GP.Domain.Common.Configurations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GP.Domain.Entities.Identity
{
    public class Permission : Entity
    {
        [Key]
        [Required]
        [StringLength(32)]
        public string Label { get; set; }

        [StringLength(32)]
        public string VisibleLabel { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
        public bool IsDirective { get; set; }

        public ICollection<PermissionCategoryPermission> Categories { get; set; }
        public Permission() { Categories = new Collection<PermissionCategoryPermission>(); }
    }
}
