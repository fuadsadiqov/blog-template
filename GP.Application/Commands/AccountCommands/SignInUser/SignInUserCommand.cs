using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.AccountCommands.SignInUser
{
    public class SignInUserCommand : CommandBase<SignInUserResponse>
    {
        public SignInUserCommand(SignInUserRequest request)
        {
            Request = request;
        }
        public SignInUserRequest Request { get; set; }
    }
}
