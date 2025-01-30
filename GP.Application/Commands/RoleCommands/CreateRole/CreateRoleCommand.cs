using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Configurations;

namespace GP.Application.Commands.RoleCommands.CreateRole
{
    public class CreateRoleCommand : CommandBase<CreateRoleResponse>, ITransactionalRequest

    {
        public CreateRoleCommand(CreateRoleRequest request)
        {
            Request = request;
        }

        public CreateRoleRequest Request { get; set; }
    }
}
