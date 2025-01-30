using GP.Domain.Common.Configurations;

namespace GP.Domain.Entities.App
{
    public class AppDomain : Entity
    {
        public int Id { get; set; }
        public string Domain { get; set; }
    }
}
