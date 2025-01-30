using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.LockUser
{
    public class LockUserCommand : CommandBase<LockUserResponse>
    {
        public LockUserCommand(LockUserRequest request)
        {
            Request = request;
        }

        public LockUserRequest Request { get; set; }
    }
}
