using AutoMapper;
using GP.Application.BlogQueries;
using GP.Application.BlogQueries.GetAllBlogsQuery;
using GP.Core.Models;
using GP.DataAccess.Repository.BlogRepository;
using GP.Domain.Entities.Common;
using GP.MVC.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BlogController : BaseController
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        public BlogController(ILogger<BlogController> logger, IBlogRepository blogRepository, IMapper mapper)
        {
            _logger = logger;
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new GetAllBlogsQuery(new GetAllBlogsRequest()));
            return View(result.BlogResponses);
        }

        public async Task<IActionResult> Detail([FromQuery(Name="id")] Guid id)
        {
            string[] includes = { "Category", "Tags.Tag" };
            var blog = await _blogRepository.GetFirstAsync(b => b.Id == id, includes);
            if(blog is null)
            {
                return RedirectToAction(nameof(Index));
            }
            var result = _mapper.Map<Blog, BlogDetailModel>(blog);

            return View(result);
        }

        public async Task<IActionResult> Add()
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
