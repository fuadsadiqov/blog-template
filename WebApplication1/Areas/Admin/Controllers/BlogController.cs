using AutoMapper;
using GP.Application.BlogQueries.GetAllBlogsQuery;
using GP.Core.Models;
using GP.DataAccess.Repository.BlogRepository;
using GP.Domain.Entities.Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GP.Application.Commands.BlogCommands.AddBlog;
using GP.Application.Commands.BlogCommands.DeleteBlog;
using GP.Application.Commands.BlogCommands.UpdateBlog;
using GP.DataAccess.Repository.CategoryRepository;
using GP.DataAccess.Repository.TagRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GP.MVC.Areas.Home.Controllers;
using GP.MVC.Areas.Home.Models;

namespace GP.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : BaseController
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public BlogController(ILogger<BlogController> logger, IBlogRepository blogRepository, IMapper mapper, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            _blogRepository = blogRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
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

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAll().ToListAsync();
            var tags = await _tagRepository.GetAll().ToListAsync();
            List<SelectListItem> tagList = new List<SelectListItem>();
            foreach (var tag in tags)
            {
                tagList.Add(new SelectListItem{ Value = tag.Id.ToString(), Text = tag.Name });
            }
            BlogViewModel blogViewModel = new BlogViewModel();
            
            blogViewModel.Categories = categories;
            blogViewModel.Tags = tagList;
            return View(blogViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddBlogRequest blogForm)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(new AddBlogCommand(blogForm));
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            string[] includes = { "Category", "Tags.Tag" };
            var blog = await _blogRepository.GetFirstAsync(c => c.Id == id, includes);
            if (blog != null)
            {
                var categories = await _categoryRepository.GetAll().ToListAsync();
                var tags = await _tagRepository.GetAll().ToListAsync();
                List<SelectListItem> tagList = new List<SelectListItem>();
                foreach (var tag in tags)
                {
                    var blogTag = blog.Tags.Where(bT => bT.TagId == tag.Id);
                    tagList.Add(new SelectListItem{ Value = tag.Id.ToString(), Text = tag.Name, Selected = blogTag.Any() });
                }
                BlogViewModel blogViewModel = new BlogViewModel();
            
                blogViewModel.Categories = categories;
                blogViewModel.Blog = blog;
                blogViewModel.Tags = tagList;
                return View(blogViewModel);
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] UpdateBlogRequest blogForm)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(new UpdateBlogCommand(blogForm));
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            DeleteBlogRequest request = new DeleteBlogRequest(){ Id = id };
            
            await Mediator.Send(new DeleteBlogCommand(request));
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
