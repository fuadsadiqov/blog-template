using GP.Core.Configurations.Entity;
using GP.Core.Enums.Enitity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace GP.Domain.Entities.Common
{
    public class Blog : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public int ViewCount { get; set; } = 0;
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        [InverseProperty("Blog")]
        public RecordStatusEnum Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public Category Category { get; set; }
        public ICollection<BlogTag> Tags { get; set; } = new Collection<BlogTag>();
    }
}
