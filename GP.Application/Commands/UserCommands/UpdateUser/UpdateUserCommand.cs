using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Configurations;

namespace GP.Application.Commands.UserCommands.UpdateUser
{
    public class UpdateUserCommand : CommandBase<UpdateUserResponse>, ITransactionalRequest
    {
        public UpdateUserCommand(UpdateUserRequest request)
        {
            Request = request;
        }

        public UpdateUserRequest Request { get; set; }
    }
}
