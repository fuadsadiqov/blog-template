using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.SetUserStatus
{
    public class SetUserStatusCommand : CommandBase<SetUserStatusResponse>
    {
        public SetUserStatusCommand(SetUserStatusRequest request)
        {
            Request = request;
        }

        public SetUserStatusRequest Request { get; set; }
    }
}
