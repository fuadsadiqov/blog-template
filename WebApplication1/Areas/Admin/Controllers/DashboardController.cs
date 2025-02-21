using GP.Application.BlogQueries.GetAllBlogsQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GP.MVC.Areas.Home.Controllers;
using GP.MVC.Areas.Home.Models;

namespace WebApplication1.Controllers
{
    [Area("Admin")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
