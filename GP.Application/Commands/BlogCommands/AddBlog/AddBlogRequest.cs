using GP.Application.Commands.BlogCommands.SetBlogTag;

namespace GP.Application.Commands.BlogCommands.AddBlog
{
    public class AddBlogRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public Guid CategoryId { get; set; }
        public SetBlogTagRequest Tags { get; set; }
    }

    
}
