using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GP.Application.Commands.CategoryCommands;
using GP.Application.CategoryQueries.GetAllCategoriesQuery;
using GP.Application.Commands.CategoryCommands.AddCategoryCommands;
using GP.Application.Commands.CategoryCommands.DeleteCategoryCommands;
using GP.Application.BlogQueries.GetAllBlogsQuery;
using GP.Application.Commands.BlogCommands.AddBlog;
using GP.Application.Commands.BlogCommands.DeleteBlog;

namespace GP.API.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class BlogController : BaseController
    {

        [Produces("application/json")]
        [HttpGet("get-all-blogs")]
        public async Task<GetAllBlogsResponse> GetAllBlogs()
        {
            var result = await Mediator.Send(new GetAllBlogsQuery(new GetAllBlogsRequest()));
            return result;
        }

        [Produces("application/json")]
        [HttpPost("add-blog")]
        public async Task<AddBlogResponse> AddBlog([FromBody] AddBlogRequest request)
        {
            var result = await Mediator.Send(new AddBlogCommand(request));
            return result;
        }

        [Produces("application/json")]
        [HttpDelete("delete-blog")]
        public async Task<DeleteBlogResponse> DeleteBlog([FromQuery] DeleteBlogRequest request)
        {
            var result = await Mediator.Send(new DeleteBlogCommand(request));
            return result;
        }
    }
}