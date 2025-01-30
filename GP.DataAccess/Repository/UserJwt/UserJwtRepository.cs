using GP.Data;

namespace GP.DataAccess.Repository.UserJwt
{
    public class UserJwtRepository
        : Repository<Domain.Entities.Identity.UserJwt>,
            IUserJwtRepository,
            IRepositoryIdentifier
    {
        public UserJwtRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
