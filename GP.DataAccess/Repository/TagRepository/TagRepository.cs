using GP.Data;
using GP.Domain.Entities.Common;


namespace GP.DataAccess.Repository.TagRepository
{
    public class TagRepository : Repository<Tag>, ITagRepository, IRepositoryIdentifier
    {
        private readonly ApplicationDbContext _context;
        public TagRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
