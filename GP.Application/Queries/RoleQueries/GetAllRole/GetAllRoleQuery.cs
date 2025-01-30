using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.RoleQueries.GetAllRole
{
    public class GetAllRoleQuery : IQuery<GetAllRoleResponse>
    {
        public GetAllRoleQuery(GetAllRoleRequest request)
        {
            Request = request;
        }

        public GetAllRoleRequest Request { get; set; }
    }
}
