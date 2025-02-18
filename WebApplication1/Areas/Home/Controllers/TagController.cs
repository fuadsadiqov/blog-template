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
using GP.Application.CategoryQueries.GetAllCategoriesQuery;
using GP.Application.Commands.CategoryCommands.AddCategoryCommands;
using GP.Application.Commands.CategoryCommands.DeleteCategoryCommands;
using GP.Application.Commands.CategoryCommands.UpdateCategoryCommands;
using GP.Application.Commands.TagCommands.AddTag;
using GP.DataAccess.Repository.CategoryRepository;
using GP.DataAccess.Repository.TagRepository;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Area("Home")]
    public class TagController : BaseController
    {
        private readonly ILogger<TagController> _logger;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public TagController(ILogger<TagController> logger, IMapper mapper, ITagRepository tagRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _tagRepository = tagRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddTagRequest request)
        {
            var response =  await Mediator.Send(new AddTagCommand(request));
            if (response.Id != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
