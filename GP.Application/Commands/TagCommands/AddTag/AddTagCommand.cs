using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.TagCommands.AddTag
{
    public class AddTagCommand : CommandBase<AddTagResponse>
    {
        public AddTagCommand(AddTagRequest request)
        {
            Request = request;
        }
        public AddTagRequest Request { get; set; }
    }
}
