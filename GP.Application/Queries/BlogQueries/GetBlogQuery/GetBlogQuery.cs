using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.BlogQueries.GetBlogQuery
{
    public class GetBlogQuery : IQuery<GetBlogResponse>
    {
        public GetBlogQuery(GetBlogRequest request)
        {
            Request = request;
        }

        public GetBlogRequest Request { get; set; }

    }
}
