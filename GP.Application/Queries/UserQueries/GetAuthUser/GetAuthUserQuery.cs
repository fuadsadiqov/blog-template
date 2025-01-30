using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.UserQueries.GetAuthUser
{
    public class GetAuthUserQuery : IQuery<GetAuthUserResponse>

    {
        public GetAuthUserQuery(GetAuthUserRequest request)
        {
            Request = request;
        }

        public GetAuthUserRequest Request { get; set; }
    }
}
