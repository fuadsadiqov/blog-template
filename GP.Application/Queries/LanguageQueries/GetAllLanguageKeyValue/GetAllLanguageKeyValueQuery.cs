using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.LanguageQueries.GetAllLanguageKeyValue
{
    public class GetAllLanguageKeyValueQuery : IQuery<GetAllLanguageKeyValueResponse>
    {
        public GetAllLanguageKeyValueRequest Request { get; }

        public GetAllLanguageKeyValueQuery(GetAllLanguageKeyValueRequest request)
        {
            Request = request;
        }
    }
}
