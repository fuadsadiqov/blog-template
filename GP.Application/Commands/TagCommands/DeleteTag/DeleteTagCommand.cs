using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.TagCommands.DeleteTagCommands
{
    public class DeleteTagCommand : CommandBase<DeleteTagResponse>
    {
        public DeleteTagCommand(DeleteTagRequest request)
        {
            Request = request;
        }
        public DeleteTagRequest Request { get; set; }
    }
}
