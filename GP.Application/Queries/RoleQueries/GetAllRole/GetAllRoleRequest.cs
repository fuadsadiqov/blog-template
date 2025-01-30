using GP.Core.Models;

namespace GP.Application.Queries.RoleQueries.GetAllRole
{
    public class GetAllRoleRequest
    {
        public RoleFilterParameters FilterParameters { get; set; }
        public PagingParameters PagingParameters { get; set; }
        public List<SortParameters> SortParameters { get; set; }
    }
}
