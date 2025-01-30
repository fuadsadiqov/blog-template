using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Configurations;

namespace GP.Application.Commands.UserCommands.ResetPassword
{
    public class ResetPasswordCommand : CommandBase<ResetPasswordResponse>, ITransactionalRequest
    {
        public ResetPasswordCommand(ResetPasswordRequest request)
        {
            Request = request;
        }

        public ResetPasswordRequest Request { get; set; }
    }
}
