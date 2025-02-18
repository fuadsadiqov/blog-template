
using GP.Application.BlogTagQueries;
using GP.Domain.Entities.Common;

namespace  GP.Application.BlogQueries
{
    public class BlogResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public string Image => $"http://localhost:7090/public/{CoverImage}";
        public string Category{ get; set; }
        public string ViewCount{ get; set; }
        public ICollection<BlogTag> Tags{ get; set; }
        public DateTime DateCreated{ get; set; }
        public string Date => DateCreated.ToString("MMMM dd, yyyy");
    }
}
