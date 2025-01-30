using GP.Data;
using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {
        public static List<UserPermission> BuildUserDirectivePermissionsList(ApplicationDbContext context)
        {
            var list = context.Permissions.Where(c => c.IsDirective);

            return list.Select(c => new UserPermission()
            {
                PermissionId = c.Label
            }).ToList();
        }
    }
}
