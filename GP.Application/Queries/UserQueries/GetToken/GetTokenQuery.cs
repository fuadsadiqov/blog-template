using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.UserQueries.GetToken
{
    public class GetTokenQuery : IQuery<GetTokenResponse>
    {
        public GetTokenQuery(GetTokenRequest request)
        {
            Request = request;
        }

        public GetTokenRequest Request { get; set; }

    }
}
