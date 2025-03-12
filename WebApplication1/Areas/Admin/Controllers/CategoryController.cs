using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GP.Application.CategoryQueries.GetAllCategoriesQuery;
using GP.Application.Commands.CategoryCommands.AddCategoryCommands;
using GP.Application.Commands.CategoryCommands.DeleteCategoryCommands;
using GP.Application.Commands.CategoryCommands.UpdateCategoryCommands;
using GP.DataAccess.Repository.CategoryRepository;
using GP.MVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;

namespace GP.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : BaseController
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new GetAllCategoriesQuery(new GetAllCategoriesRequest()));
            return View(result.CategoryResponses);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddCategoryRequest categoryForm)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(new AddCategoryCommand(categoryForm));
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryRepository.GetFirstAsync(c => c.Id == id);
            if (category != null)
            {
                return View(category);
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] UpdateCategoryRequest categoryForm)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(new UpdateCategoryCommand(categoryForm));
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            DeleteCategoryRequest request = new DeleteCategoryRequest{ Id = id };
            
            await Mediator.Send(new DeleteCategoryCommand(request));
            return RedirectToAction("Index");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
