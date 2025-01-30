using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.LanguageCommands.DeleteLanguageKeyValue
{
    public class DeleteLanguageKeyValueCommand : CommandBase<DeleteLanguageKeyValueResponse>
    {
        public DeleteLanguageKeyValueRequest Request { get; }

        public DeleteLanguageKeyValueCommand(DeleteLanguageKeyValueRequest request)
        {
            Request = request;
        }
    }
}
