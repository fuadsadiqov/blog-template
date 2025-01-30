using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Configurations;

namespace GP.Application.Commands.RoleCommands.UpdateRole
{
    public class UpdateRoleCommand : CommandBase<UpdateRoleResponse>, ITransactionalRequest
    {
        public UpdateRoleCommand(UpdateRoleRequest request)
        {
            Request = request;
        }

        public UpdateRoleRequest Request { get; set; }
    }
}
