using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GP.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
