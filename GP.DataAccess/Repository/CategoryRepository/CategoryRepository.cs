using GP.Data;
using GP.Domain.Entities.Common;


namespace GP.DataAccess.Repository.CategoryRepository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository, IRepositoryIdentifier
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
