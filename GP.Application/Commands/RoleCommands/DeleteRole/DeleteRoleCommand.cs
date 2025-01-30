using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.RoleCommands.DeleteRole
{
    public class DeleteRoleCommand : CommandBase<DeleteRoleResponse>
    {
        public DeleteRoleCommand(DeleteRoleRequest request)
        {
            Request = request;
        }

        public DeleteRoleRequest Request { get; set; }
    }
}
