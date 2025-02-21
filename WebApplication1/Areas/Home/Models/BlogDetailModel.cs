using GP.Application.BlogQueries;

namespace GP.MVC.Areas.Home.Models;

public class BlogDetailModel
{
    public List<BlogResponse> lastBlogs { get; set; }
    public BlogResponse blog { get; set; }
}