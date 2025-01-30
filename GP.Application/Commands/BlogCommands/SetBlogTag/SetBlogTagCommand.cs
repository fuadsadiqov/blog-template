using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.BlogCommands.SetBlogTag
{
    public class SetBlogTagCommand : CommandBase<SetBlogTagResponse>
    {
        public SetBlogTagCommand(SetBlogTagRequest request)
        {
            Request = request;
        }
        public SetBlogTagRequest Request { get; set; }
    }
}
