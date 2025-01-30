using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {

        public static List<Role> BuildRoleList()
        {
            var list = new List<Role>()
            {
                new Role() { Id = Guid.NewGuid().ToString(), Name = "Admin" }

            };

            return list;
        }
    }
}
