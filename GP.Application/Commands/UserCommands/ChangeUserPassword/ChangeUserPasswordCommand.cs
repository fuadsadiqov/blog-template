using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.UserCommands.ChangeUserPassword
{
    public class ChangeUserPasswordCommand : CommandBase<ChangeUserPasswordResponse>
    {
        public ChangeUserPasswordCommand(string requestedUserId,
            bool changeWithoutOldPassword, ChangeUserPasswordRequest request)
        {
            RequestedUserId = requestedUserId;
            Request = request;
            ChangeWithoutOldPassword = changeWithoutOldPassword;
        }
        public string RequestedUserId { get; set; }
        public bool ChangeWithoutOldPassword { get; set; }
        public ChangeUserPasswordRequest Request { get; set; }
    }
}
