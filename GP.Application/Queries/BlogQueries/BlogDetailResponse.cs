using GP.Domain.Entities.Common;
using GP.Domain.Entities.Identity;

namespace GP.Application.Queries.BlogQueries
{
    public class BlogDetailResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public string Image => $"http://localhost:7090/public/{CoverImage}";
        public BlogCategoryResponse Category { get; set; }
        public string ViewCount { get; set; }
        public string Tags { get; set; }
        public ICollection<BlogReview> Reviews { get; set; }
        public DateTime DateCreated{ get; set; }
        public string Date => DateCreated.ToString("dd.MM.yyyy - HH:mm");
    }
    
    public class BlogCategoryResponse
    {
        public Guid Id { get; set; }
        public string Title{ get; set; }
    }

    public class BlogReview
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public BlogReviewUser User { get; set; }
        public Guid BlogId { get; set; }
        public string Date => DateCreated.ToString("dd.MM.yyyy - HH:mm");
    }

    public class BlogReviewUser
    {
        public string Id { get; set; }   
        public string FullName { get; set; }
        public bool IsAuthReview { get; set; }
    }
}
