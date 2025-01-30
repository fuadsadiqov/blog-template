using GP.Data;
using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Repository.PermissionRepository
{
    public class PermissionCategoryPermissionRepository : Repository<PermissionCategoryPermission>, IPermissionCategoryPermissionRepository, IRepositoryIdentifier
    {
        public PermissionCategoryPermissionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
