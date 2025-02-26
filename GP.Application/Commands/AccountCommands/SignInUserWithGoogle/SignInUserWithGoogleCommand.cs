using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.AccountCommands.SignInUserWithGoogle
{
    public class SignInUserWithGoogleCommand : CommandBase<SignInUserWithGoogleResponse>
    {
        public SignInUserWithGoogleCommand(SignInUserWithGoogleRequest request)
        {
            Request = request;
        }
        public SignInUserWithGoogleRequest Request { get; set; }
    }
}
