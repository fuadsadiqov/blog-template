using GP.Application.BlogQueries.GetAllBlogsQuery;
using GP.Domain.Entities.Common;
using GP.MVC.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
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
