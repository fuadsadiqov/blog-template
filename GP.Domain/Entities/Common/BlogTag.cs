using GP.Core.Configurations.Entity;
using GP.Core.Enums.Enitity;

namespace GP.Domain.Entities.Common
{
    public class BlogTag : IEntity
    {
        public Guid BlogId { get; set; }
        public Guid TagId { get; set; }
        public RecordStatusEnum Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public Blog Blog { get; set; }
        public Tag Tag { get; set; }
    }
}
