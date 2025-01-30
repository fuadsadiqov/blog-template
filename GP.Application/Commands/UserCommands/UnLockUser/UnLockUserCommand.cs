using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.UnLockUser
{
    public class UnLockUserCommand(UnLockUserRequest request) : CommandBase<UnLockUserResponse>
    {
        public UnLockUserRequest Request { get; set; } = request;
    }
}
