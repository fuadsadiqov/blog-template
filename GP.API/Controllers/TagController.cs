using GP.Application.Commands.TagCommands.AddTag;
using GP.Application.Commands.TagCommands.DeleteTagCommands;
using GP.Application.TagQueries.GetAllTagsQuery;
using Microsoft.AspNetCore.Mvc;

namespace GP.API.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class TagController : BaseController
    {

        [Produces("application/json")]
        [HttpGet("get-all-tags")]
        public async Task<GetAllTagsResponse> GetAllTags()
        {
            var result = await Mediator.Send(new GetAllTagsQuery(new GetAllTagsRequest()));
            return result;
        }

        [Produces("application/json")]
        [HttpPost("add-tag")]
        public async Task<AddTagResponse> AddTag([FromBody] AddTagRequest request)
        {
            var result = await Mediator.Send(new AddTagCommand(request));
            return result;
        }

        [Produces("application/json")]
        [HttpDelete("delete-tag")]
        public async Task<DeleteTagResponse> DeleteTag([FromQuery] DeleteTagRequest request)
        {
            var result = await Mediator.Send(new DeleteTagCommand(request));
            return result;
        }
    }
}