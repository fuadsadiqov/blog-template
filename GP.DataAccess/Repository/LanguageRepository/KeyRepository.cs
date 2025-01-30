using GP.Data;
using GP.Domain.Entities.Lang;

namespace GP.DataAccess.Repository.LanguageRepository
{
    public class KeyRepository : Repository<Key>, IKeyRepository, IRepositoryIdentifier
    {
        public KeyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

    public interface ILanguageKeyRepository : IRepository<LanguageKey>
    {

    }

    public class LanguageKeyRepository : Repository<LanguageKey>, ILanguageKeyRepository, IRepositoryIdentifier
    {
        public LanguageKeyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
