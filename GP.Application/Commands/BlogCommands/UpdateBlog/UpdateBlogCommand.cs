using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.BlogCommands.UpdateBlog
{
    public class UpdateBlogCommand : CommandBase<UpdateBlogResponse>
    {
        public UpdateBlogCommand(UpdateBlogRequest request)
        {
            Request = request;
        }
        public UpdateBlogRequest Request { get; set; }
    }
}
