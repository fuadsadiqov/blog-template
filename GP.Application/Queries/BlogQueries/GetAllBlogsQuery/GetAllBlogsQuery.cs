using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.BlogQueries.GetAllBlogsQuery
{
    public class GetAllBlogsQuery : IQuery<GetAllBlogsResponse>
    {
        public GetAllBlogsQuery(GetAllBlogsRequest request)
        {
            Request = request;
        }

        public GetAllBlogsRequest Request { get; set; }

    }
}
