using GP.Application.BlogQueries.GetAllBlogsQuery;
using GP.Application.BlogQueries.GetBlogQuery;
using GP.MVC.Areas.Home.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GP.Application.Commands.BlogCommands.UpdateBlogViewCount;
using GP.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using GP.Application.Commands.ReviewCommands.AddReviewCommand;
using GP.Application.Commands.ReviewCommands.DeleteReviewCommand;
using GP.Application.Queries.UserQueries.GetAuthUser;

namespace GP.MVC.Areas.Home.Controllers
{

    [Area("Home")]
    public class ReviewController : BaseController
    {
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ILogger<ReviewController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/Review/Delete/{blogId}/{id}")]
        public async Task<IActionResult> Delete(Guid id, Guid blogId)
        {
            var review = await Mediator.Send(new DeleteReviewCommand(new DeleteReviewRequest{ Id = id }));
            return RedirectToAction("Detail", "Home", new { area = "Home", id = blogId });
        }
    }
}
