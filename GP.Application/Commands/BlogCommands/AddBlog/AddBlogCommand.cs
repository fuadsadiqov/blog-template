using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.BlogCommands.AddBlog
{
    public class AddBlogCommand : CommandBase<AddBlogResponse>
    {
        public AddBlogCommand(AddBlogRequest request)
        {
            Request = request;
        }
        public AddBlogRequest Request { get; set; }
    }
}
