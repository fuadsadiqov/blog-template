using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.BlogTagQueries.GetAllBlogTagsQuery
{
    public class GetAllBlogTagsQuery : IQuery<GetAllBlogTagsResponse>
    {
        public GetAllBlogTagsQuery(GetAllBlogTagsRequest request)
        {
            Request = request;
        }

        public GetAllBlogTagsRequest Request { get; set; }

    }
}
