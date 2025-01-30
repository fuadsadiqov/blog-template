using GP.Data;
using AppDomain = GP.Domain.Entities.App.AppDomain;

namespace GP.DataAccess.Repository.AppDomainRepository
{
    public class AppDomainRepository : Repository<AppDomain>, IAppDomainRepository, IRepositoryIdentifier
    {
        public AppDomainRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
