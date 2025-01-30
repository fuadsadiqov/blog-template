using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GP.Application.Commands.CategoryCommands;
using GP.Application.CategoryQueries.GetAllCategoriesQuery;
using GP.Application.Commands.CategoryCommands.AddCategoryCommands;
using GP.Application.Commands.CategoryCommands.DeleteCategoryCommands;

namespace GP.API.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class CategoryController : BaseController
    {

        [Produces("application/json")]
        [HttpGet("get-all-categories")]
        public async Task<GetAllCategoriesResponse> GetAllCategories()
        {
            var result = await Mediator.Send(new GetAllCategoriesQuery(new GetAllCategoriesRequest()));
            return result;
        }

        [Produces("application/json")]
        [HttpPost("add-category")]
        public async Task<AddCategoryResponse> AddCategory([FromBody] AddCategoryRequest request)
        {
            var result = await Mediator.Send(new AddCategoryCommand(request));
            return result;
        }

        [Produces("application/json")]
        [HttpDelete("delete-category")]
        public async Task<DeleteCategoryResponse> DeleteCategory([FromQuery] DeleteCategoryRequest request)
        {
            var result = await Mediator.Send(new DeleteCategoryCommand(request));
            return result;
        }
    }
}