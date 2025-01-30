using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.AppOptionQueries.GetUserAppOption
{
    public class GetUserAppOptionQuery : IQuery<GetUserAppOptionResponse>
    {
        public GetUserAppOptionQuery(GetUserAppOptionRequest request)
        {
            Request = request;
        }

        public GetUserAppOptionRequest Request { get; set; }
    }
}
