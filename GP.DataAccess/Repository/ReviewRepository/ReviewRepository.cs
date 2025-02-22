using GP.Data;
using GP.Domain.Entities.Common;

namespace GP.DataAccess.Repository.ReviewRepository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository, IRepositoryIdentifier
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
