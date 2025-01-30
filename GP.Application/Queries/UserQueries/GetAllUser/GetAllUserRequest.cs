using GP.Core.Models;

namespace GP.Application.Queries.UserQueries.GetAllUser
{
    public class GetAllUserRequest
    {
        public UserFilterParameters FilterParameters { get; set; }
        public PagingParameters PagingParameters { get; set; }
        public List<SortParameters> SortParameters { get; set; }
    }
}
