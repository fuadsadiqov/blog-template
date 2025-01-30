using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.RoleCommands.SetRoleUser
{
    public class SetRoleUserCommand : CommandBase<SetRoleUserResponse>
    {
        public SetRoleUserCommand(SetRoleUserRequest request)
        {
            Request = request;
        }

        public SetRoleUserRequest Request { get; set; }
    }
}
