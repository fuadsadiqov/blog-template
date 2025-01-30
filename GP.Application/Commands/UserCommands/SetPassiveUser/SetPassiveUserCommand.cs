using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Configurations;

namespace GP.Application.Commands.UserCommands.SetPassiveUser
{
    public class SetPassiveUserCommand : CommandBase<SetPassiveUserResponse>, ITransactionalRequest
    {
        public SetPassiveUserCommand(SetPassiveUserRequest request)
        {
            Request = request;
        }

        public SetPassiveUserRequest Request { get; set; }
    }
}
