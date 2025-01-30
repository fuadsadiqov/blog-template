using GP.Core.Models;
using GP.Domain.Entities.Lang;

namespace GP.DataAccess.Repository.LanguageRepository
{
    public interface ILanguageRepository : IRepository<Language>
    {
        //get hierarchy
        List<KeyPocoModel> GetKeys(string languageCode);
        Dictionary<string, object> GetAsDictionary(string languageCode);
        List<LanguagePocoModel> GetLanguagesAsync(string languageCode);

        Task SetValueAsync(string languageId, int keyId, string value);
        Task ResetAsync();
    }
}
