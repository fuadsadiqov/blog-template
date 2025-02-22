using GP.Application.BlogQueries;
using GP.Application.BlogQueries.GetBlogQuery;
using GP.Application.Queries.BlogQueries;

namespace GP.MVC.Areas.Home.Models;

public class BlogDetailViewModel
{
    public List<BlogResponse> lastBlogs { get; set; }
    public BlogDetailResponse blog { get; set; }
}