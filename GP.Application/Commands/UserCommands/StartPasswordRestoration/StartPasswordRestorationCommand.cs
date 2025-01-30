using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Configurations;

namespace GP.Application.Commands.UserCommands.StartPasswordRestoration
{
    public class StartPasswordRestorationCommand : CommandBase<StartPasswordRestorationResponse>, ITransactionalRequest
    {
        public StartPasswordRestorationCommand(StartPasswordRestorationRequest request)
        {
            Request = request;
        }

        public StartPasswordRestorationRequest Request { get; set; }
    }
}
