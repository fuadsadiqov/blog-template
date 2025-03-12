using GP.Application.BlogQueries;

namespace GP.MVC.Areas.Home.Models;
public class HomePageResponse
{
    public List<BlogResponse> BannerBlogs { get; set; }
    public Dictionary<string, List<BlogResponse>> GroupedBlogs { get; set; }
}

public class BlogPageResponse
{
    public string Category { get; set; }
    public List<BlogResponse> BlogResponses { get; set; }
}
