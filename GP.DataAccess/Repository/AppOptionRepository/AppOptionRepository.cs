using GP.Data;
using GP.Domain.Entities.App;

namespace GP.DataAccess.Repository.AppOptionRepository
{
    public class AppOptionRepository : Repository<AppOption>, IAppOptionRepository, IRepositoryIdentifier
    {
        public AppOptionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
