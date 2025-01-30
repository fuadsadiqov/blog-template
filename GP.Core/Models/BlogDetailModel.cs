namespace GP.Core.Models
{
    public class BlogDetailModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public string Image => $"http://localhost:7090/public/{CoverImage}";
        public string Category { get; set; }
        public string ViewCount { get; set; }
        public string Tags { get; set; }
    }
}
