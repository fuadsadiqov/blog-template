using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.LanguageCommands.SetLanguageKeyValue
{
    public class SetLanguageKeyValueCommand : CommandBase<SetLanguageKeyValueResponse>
    {
        public SetLanguageKeyValueRequest Request { get; }

        public SetLanguageKeyValueCommand(SetLanguageKeyValueRequest request)
        {
            Request = request;
        }
    }
}
