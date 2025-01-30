using GP.Data;
using GP.Domain.Entities.Common;


namespace GP.DataAccess.Repository.BlogRepository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository, IRepositoryIdentifier
    {
        private readonly ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
