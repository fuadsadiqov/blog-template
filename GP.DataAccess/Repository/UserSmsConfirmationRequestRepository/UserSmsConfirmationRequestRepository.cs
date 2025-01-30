using GP.Data;
using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Repository.UserSmsConfirmationRequestRepository
{
    public class UserSmsConfirmationRequestRepository : Repository<UserSmsConfirmationRequest>, IUserSmsConfirmationRequestRepository, IRepositoryIdentifier
    {

        public UserSmsConfirmationRequestRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
