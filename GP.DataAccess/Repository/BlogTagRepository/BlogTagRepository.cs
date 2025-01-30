using GP.Data;
using GP.Domain.Entities.Common;


namespace GP.DataAccess.Repository.BlogTagRepository
{
    public class BlogTagRepository : Repository<BlogTag>, IBlogTagRepository, IRepositoryIdentifier
    {
        private readonly ApplicationDbContext _context;
        public BlogTagRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
