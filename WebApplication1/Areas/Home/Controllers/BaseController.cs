using GP.Application.CategoryQueries.GetAllCategoriesQuery;
using GP.DataAccess.Repository.CategoryRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GP.MVC.Controllers
{
    [Area("Home")]
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
