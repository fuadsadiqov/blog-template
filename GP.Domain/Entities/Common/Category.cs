using GP.Domain.Common.Configurations;

namespace GP.Domain.Entities.Common
{
    public class Category : Entity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
