using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.SetUserPermission
{
    public class SetUserPermissionCommand : CommandBase<SetUserPermissionResponse>
    {
        public SetUserPermissionCommand(SetUserPermissionRequest request)
        {
            Request = request;
        }

        public SetUserPermissionRequest Request { get; set; }
    }
}
