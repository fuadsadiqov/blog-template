using GP.Application.BlogQueries.GetAllBlogsQuery;
using GP.Application.BlogQueries.GetBlogQuery;
using GP.MVC.Areas.Home.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GP.Application.Commands.BlogCommands.UpdateBlogViewCount;
using GP.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using GP.Application.Commands.ReviewCommands.AddReviewCommand;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NToastNotify;

namespace GP.MVC.Areas.Home.Controllers
{
    public class ReviewForm
    {
        public string Message{ get; set; }
        public Guid BlogId { get; set; }
    }

    [Area("Home")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthService _authService;
        private readonly IToastNotification _toastNotification;

        public HomeController(ILogger<HomeController> logger, AuthService authService, IToastNotification toastNotification)
        {
            _logger = logger;
            _authService = authService;
            _toastNotification = toastNotification;
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
            
            GetBlogRequest request = new GetBlogRequest{ Id = id };
            var blog = await Mediator.Send(new GetBlogQuery(request));
            var lastBlogs = await Mediator.Send(new GetAllBlogsQuery(new GetAllBlogsRequest()));

            var userId = _authService.GetAuthorizedUserId();

            if (userId != null && blog.BlogResponse.Reviews != null && blog.BlogResponse.Reviews.Any())
            {
                var authUserReview = blog.BlogResponse.Reviews.FirstOrDefault(r => r.User.Id == userId);
                if (authUserReview != null)
                {
                    authUserReview.User.IsAuthReview = true;
                }
            }
            
            BlogDetailViewModel model = new BlogDetailViewModel{ lastBlogs = lastBlogs.BlogResponses, blog = blog.BlogResponse };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Review([FromForm] ReviewForm reviewForm)
        {
            var message = reviewForm.Message;
            var blogId = reviewForm.BlogId;
            var email = User.Identity.Name;

            var response = await Mediator.Send(new AddReviewCommand(new AddReviewRequest() { Email = email, BlogId = blogId, Message = message }));
            if (!response.IsSuccedd)
            {
                _toastNotification.AddErrorToastMessage(response.Message);
            }
            else
            {
                _toastNotification.AddSuccessToastMessage(response.Message);
            }
            return RedirectToAction("Detail", new { id = blogId });
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
