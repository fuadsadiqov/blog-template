using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.TagQueries.GetAllTagsQuery
{
    public class GetAllTagsQuery : IQuery<GetAllTagsResponse>
    {
        public GetAllTagsQuery(GetAllTagsRequest request)
        {
            Request = request;
        }

        public GetAllTagsRequest Request { get; set; }

    }
}
