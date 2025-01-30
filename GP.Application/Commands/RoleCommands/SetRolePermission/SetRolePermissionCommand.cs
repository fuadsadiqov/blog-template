using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.RoleCommands.SetRolePermission
{
    public class SetRolePermissionCommand : CommandBase<SetRolePermissionResponse>
    {
        public SetRolePermissionCommand(SetRolePermissionRequest request)
        {
            Request = request;
        }

        public SetRolePermissionRequest Request { get; set; }
    }
}
