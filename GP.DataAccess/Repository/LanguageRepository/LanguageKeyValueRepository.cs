using GP.Data;
using GP.Domain.Entities.Lang;

namespace GP.DataAccess.Repository.LanguageRepository
{
    public class LanguageKeyValueRepository : Repository<LanguageKeyValue>, ILanguageKeyValueRepository, IRepositoryIdentifier
    {
        public LanguageKeyValueRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
