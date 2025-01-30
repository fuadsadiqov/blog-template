using GP.Infrastructure.Configurations;
using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.CreateUser
{
    public class CreateUserCommand : CommandBase<CreateUserResponse>, ITransactionalRequest
    {
        public CreateUserCommand(CreateUserRequest request)
        {
            Request = request;
        }

        public CreateUserRequest Request { get; set; }
    }
}
