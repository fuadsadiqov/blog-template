using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.PermissionQueries.GetAllPermission
{
    public class GetAllPermissionQuery : IQuery<GetAllPermissionResponse>
    {
        public GetAllPermissionRequest Request { get; set; }
    }
}
