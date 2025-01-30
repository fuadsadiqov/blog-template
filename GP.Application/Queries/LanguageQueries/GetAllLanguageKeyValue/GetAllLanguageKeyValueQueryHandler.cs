using GP.DataAccess.Repository.LanguageRepository;
using GP.Domain.Entities.Lang;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GP.Application.Queries.LanguageQueries.GetAllLanguageKeyValue
{
    public class GetAllLanguageKeyValueQueryHandler : IQueryHandler<GetAllLanguageKeyValueQuery, GetAllLanguageKeyValueResponse>
    {
        private readonly ILanguageKeyValueRepository _languageKeyValueRepository;
        private readonly TranslationService _translationService;

        public GetAllLanguageKeyValueQueryHandler(ILanguageKeyValueRepository languageKeyValueRepository,
            TranslationService translationService)
        {
            _languageKeyValueRepository = languageKeyValueRepository;
            _translationService = translationService;
        }

        public async Task<GetAllLanguageKeyValueResponse> Handle(GetAllLanguageKeyValueQuery query,
            CancellationToken cancellationToken)
        {
            var languageCode = query.Request.LanguageCode;

            var cashedData = _translationService.GetCachedData();
            if (cashedData != null)
            {
                if (string.IsNullOrEmpty(languageCode))
                {
                    return new GetAllLanguageKeyValueResponse(cashedData);
                }

                var languageCachedData = cashedData[languageCode];
                if (languageCachedData != null)
                {
                    var resp = new Dictionary<string, Dictionary<string, string>>
                    { { languageCode, languageCachedData } };
                    return new GetAllLanguageKeyValueResponse(resp);
                }
            }

            Expression<Func<LanguageKeyValue, bool>> filterPredicate = c => true;

            if (!string.IsNullOrEmpty(languageCode))
            {
                filterPredicate = c => c.LanguageCode == languageCode;
            }

            var list = await _languageKeyValueRepository.FindBy(filterPredicate)
                .Select(c => new
                {
                    c.LanguageCode,
                    c.Value,
                    c.Key
                })
                .ToListAsync(cancellationToken);

            var response = list.GroupBy(c => c.LanguageCode)
                .ToDictionary(c => c.Key, c => c.ToDictionary(a => a.Key, a => a.Value));

            return new GetAllLanguageKeyValueResponse(response);
        }
    }
}
