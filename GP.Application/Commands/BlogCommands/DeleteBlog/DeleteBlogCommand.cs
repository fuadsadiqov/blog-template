using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.BlogCommands.DeleteBlog
{
    public class DeleteBlogCommand : CommandBase<DeleteBlogResponse>
    {
        public DeleteBlogCommand(DeleteBlogRequest request)
        {
            Request = request;
        }
        public DeleteBlogRequest Request { get; set; }
    }
}
