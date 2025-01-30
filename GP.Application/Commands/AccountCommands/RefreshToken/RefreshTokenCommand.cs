using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.AccountCommands.RefreshToken
{
    public class RefreshTokenCommand : CommandBase<RefreshTokenResponse>
    {
        public RefreshTokenCommand(RefreshTokenRequest request)
        {
            Request = request;
        }
        public RefreshTokenRequest Request { get; set; }
    }

}
