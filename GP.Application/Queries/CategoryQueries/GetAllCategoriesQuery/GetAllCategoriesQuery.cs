using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.CategoryQueries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQuery : IQuery<GetAllCategoriesResponse>
    {
        public GetAllCategoriesQuery(GetAllCategoriesRequest request)
        {
            Request = request;
        }

        public GetAllCategoriesRequest Request { get; set; }

    }
}
