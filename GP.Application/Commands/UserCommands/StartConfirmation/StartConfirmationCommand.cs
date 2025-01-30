using GP.Infrastructure.Configurations;
using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.StartConfirmation
{
    public class StartConfirmationCommand : CommandBase<StartConfirmationResponse>, ITransactionalRequest
    {
        public StartConfirmationCommand(StartConfirmationRequest request)
        {
            Request = request;
        }

        public StartConfirmationRequest Request { get; set; }
    }
}
