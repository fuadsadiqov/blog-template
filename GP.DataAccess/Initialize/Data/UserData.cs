using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {
        public static List<User> BuildUserList()
        {
            var list = new List<User>()
            {
                new User { Id = Guid.NewGuid().ToString(), UserName = "admin", Email = "admin@gmail.com", DateCreated = DateTime.Now }
            };

            return list;
        }
    }
}
