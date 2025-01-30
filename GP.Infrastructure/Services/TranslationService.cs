using GP.Core.Enums;
using GP.Core.Extensions;
using GP.DataAccess.Repository.LanguageRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GP.Infrastructure.Services
{
    public class TranslationService
    {
        private readonly IMemoryCache _cache;
        private readonly ILanguageKeyValueRepository _repository;   
        private readonly string _acceptLanguage;

        public TranslationService(IMemoryCache cache, ILanguageKeyValueRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _repository = repository;
            _acceptLanguage = httpContextAccessor.HttpContext.GetAcceptLanguage();
        }

        public async Task SetCacheAsync()
        {
            var list = await _repository.FindBy(c => true)
                .Select(c => new
                {
                    c.LanguageCode,
                    c.Value,
                    c.Key
                })
                .ToListAsync();

            var response = list.GroupBy(c => c.LanguageCode)
                .ToDictionary(c => c.Key, c => c.ToDictionary(a => a.Key, a => a.Value));

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(1))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(list.Count);

            _cache.Remove("LANG");
            _cache.Set("LANG", response, cacheEntryOptions);
        }

        public Dictionary<string, Dictionary<string, string>> GetCachedData()
        {
            if (_cache.TryGetValue("LANG", out Dictionary<string, Dictionary<string, string>> data))
                return data;
            return null;
        }

        public string Search(string key)
        {
            var data = GetCachedData();
            if (data != null)
            {
                string languageCode;
                const string az = LanguageStringEnum.az_AZ;
                const string en = LanguageStringEnum.en_US;
                switch (_acceptLanguage)
                {
                    case az:
                        languageCode = az;
                        break;
                    case en:
                        languageCode = en;
                        break;
                    default:
                        languageCode = en;
                        break;
                }

                var list = data[languageCode];
                var value = list[key];
                return value;
            }

            return string.Empty;
        }
    }
}
