using GP.Application.BlogQueries.GetAllBlogsQuery;
using GP.Application.BlogQueries.GetBlogQuery;
using GP.MVC.Areas.Home.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GP.Application.Commands.BlogCommands.UpdateBlogViewCount;

namespace GP.MVC.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await Mediator.Send(new GetAllBlogsQuery(new GetAllBlogsRequest()));
            var groupedBlogs = blogs.BlogResponses.GroupBy(b => b.Category.Title)
                                                   .ToDictionary(g => g.Key, g => g.ToList());

            return View(groupedBlogs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail( Guid id)
        {
            UpdateBlogViewCountRequest updateRequest = new UpdateBlogViewCountRequest{ Id = id };
            await Mediator.Send(new UpdateBlogViewCountCommand(updateRequest));
            
            GetBlogRequest request = new GetBlogRequest() { Id = id };
            var blog = await Mediator.Send(new GetBlogQuery(request));
            var lastBlogs = await Mediator.Send(new GetAllBlogsQuery(new GetAllBlogsRequest()));

            BlogDetailModel model = new BlogDetailModel(){ lastBlogs = lastBlogs.BlogResponses, blog = blog.BlogResponses };
            
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
