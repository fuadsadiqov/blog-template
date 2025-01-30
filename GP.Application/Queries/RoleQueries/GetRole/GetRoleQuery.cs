using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.RoleQueries.GetRole
{
    public class GetRoleQuery : IQuery<GetRoleResponse>
    {
        public GetRoleQuery(GetRoleRequest request)
        {
            Request = request;
        }

        public GetRoleRequest Request { get; set; }

    }
}
