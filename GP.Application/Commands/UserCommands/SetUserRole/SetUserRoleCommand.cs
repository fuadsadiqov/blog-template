using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.SetUserRole
{
    public class SetUserRoleCommand : CommandBase<SetUserRoleResponse>
    {
        public SetUserRoleCommand(SetUserRoleRequest request)
        {
            Request = request;
        }

        public SetUserRoleRequest Request { get; set; }
    }
}
