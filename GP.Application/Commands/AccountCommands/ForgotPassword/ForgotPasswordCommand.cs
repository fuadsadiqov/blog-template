using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.AccountCommands.ForgotPassword
{
    public class ForgotPasswordCommand : CommandBase<ForgotPasswordResponse>
    {
        public ForgotPasswordCommand(ForgotPasswordRequest request)
        {
            Request = request;
        }
        public ForgotPasswordRequest Request { get; set; }
    }
}
