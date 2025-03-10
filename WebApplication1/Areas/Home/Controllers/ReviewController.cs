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
using NToastNotify;

namespace GP.MVC.Areas.Home.Controllers
{

    [Area("Home")]
    public class ReviewController : BaseController
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IToastNotification _toastNotification;

        public ReviewController(ILogger<ReviewController> logger, IToastNotification toastNotification)
        {
            _logger = logger;
            _toastNotification = toastNotification;
        }

        [HttpGet("/Review/Delete/{blogId}/{id}")]
        public async Task<IActionResult> Delete(Guid id, Guid blogId)
        {
            var review = await Mediator.Send(new DeleteReviewCommand(new DeleteReviewRequest{ Id = id }));
            if (!review.IsSuccedd)
            {
                _toastNotification.AddErrorToastMessage(review.Message);
            }
            else
            {
                _toastNotification.AddSuccessToastMessage(review.Message);
            }
            return RedirectToAction("Detail", "Home", new { area = "Home", id = blogId });
        }
    }
}
