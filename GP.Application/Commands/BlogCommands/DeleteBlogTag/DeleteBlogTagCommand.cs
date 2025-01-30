using GP.Application.Commands.BlogCommands.DeleteBlog;
using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.BlogCommands.DeleteBlogTag
{
    public class DeleteBlogTagCommand : CommandBase<DeleteBlogTagResponse>
    {
        public DeleteBlogTagCommand(DeleteBlogRequest request)
        {
            Request = request;
        }
        public DeleteBlogRequest Request { get; set; }
    }
}
