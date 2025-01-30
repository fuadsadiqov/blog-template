using GP.Data;
using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Repository.ConfirmationRepository
{
    public class ConfirmationRepository : Repository<Confirmation>, IConfirmationRepository, IRepositoryIdentifier
    {
        private readonly ApplicationDbContext _context;
        public ConfirmationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
