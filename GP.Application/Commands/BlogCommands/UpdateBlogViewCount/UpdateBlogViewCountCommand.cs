using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.BlogCommands.UpdateBlogViewCount
{
    public class UpdateBlogViewCountCommand : CommandBase<UpdateBlogViewCountResponse>
    {
        public UpdateBlogViewCountCommand(UpdateBlogViewCountRequest request)
        {
            Request = request;
        }
        public UpdateBlogViewCountRequest Request { get; set; }
    }
}
