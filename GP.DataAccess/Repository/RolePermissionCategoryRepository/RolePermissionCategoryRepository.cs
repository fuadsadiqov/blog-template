using GP.Data;
using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Repository.RolePermissionCategoryRepository
{
    public class RolePermissionCategoryRepository : Repository<RolePermissionCategory>, IRolePermissionCategoryRepository, IRepositoryIdentifier
    {
        public RolePermissionCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
