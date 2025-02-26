using GP.Infrastructure.Configurations;
using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.CreateUserWithGoogle
{
    public class CreateUserWithGoogleCommand : CommandBase<CreateUserWithGoogleResponse>, ITransactionalRequest
    {
        public CreateUserWithGoogleCommand(CreateUserWithGoogleRequest request)
        {
            Request = request;
        }

        public CreateUserWithGoogleRequest Request { get; set; }
    }
}
