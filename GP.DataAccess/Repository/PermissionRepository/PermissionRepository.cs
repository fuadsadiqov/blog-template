using GP.Data;
using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Repository.PermissionRepository
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository, IRepositoryIdentifier
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
