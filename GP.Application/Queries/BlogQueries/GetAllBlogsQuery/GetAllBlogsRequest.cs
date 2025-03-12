using GP.Core.Models;

namespace GP.Application.BlogQueries.GetAllBlogsQuery
{
    public class GetAllBlogsRequest
    {
        public Guid? CategoryId { get; set; }
        public PagingParameters PagingParameters { get; set; }
    }
}
