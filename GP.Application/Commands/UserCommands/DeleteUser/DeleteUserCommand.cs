using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Configurations;

namespace GP.Application.Commands.UserCommands.DeleteUser
{
    public class DeleteUserCommand : CommandBase<DeleteUserResponse>, ITransactionalRequest
    {
        public DeleteUserCommand(DeleteUserRequest request)
        {
            Request = request;
        }

        public DeleteUserRequest Request { get; set; }
    }
}
