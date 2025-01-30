using GP.Core.Configurations.Entity;
using GP.Core.Enums.Enitity;
using System.Collections.ObjectModel;

namespace GP.Domain.Entities.Common
{
    public class Tag : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public ICollection<BlogTag> Blogs { get; set; } = new Collection<BlogTag>();
        public RecordStatusEnum Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
