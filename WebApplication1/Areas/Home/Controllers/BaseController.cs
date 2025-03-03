﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GP.MVC.Areas.Home.Controllers
{
    [Area("Home")]
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
