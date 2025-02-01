using GP.Application.Commands.BlogCommands.SetBlogTag;

namespace GP.Application.Commands.BlogCommands.UpdateBlog
{
    public class UpdateBlogRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public Guid CategoryId { get; set; }
        public string? Tags { get; set; }
    }

    
}
