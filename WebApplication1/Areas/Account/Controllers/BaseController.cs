using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GP.MVC.Areas.Account.Controllers
{
    [Area("Account")]
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
