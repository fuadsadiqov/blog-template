using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using GP.Application.Commands.LanguageCommands.DeleteLanguageKeyValue;
using GP.Application.Commands.LanguageCommands.SetLanguageKeyValue;
using GP.Application.Queries.LanguageQueries.GetAllLanguageKeyValue;
using GP.Infrastructure.Middlewares;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.API.Controllers.Language
{
    public class LanguageController : BaseController
    {
        private readonly TranslationService _translationService;

        public LanguageController(TranslationService translationService)
        {
            _translationService = translationService;
        }

        /// <summary>
        /// Languages
        /// </summary>
        /// <remarks>
        /// Gets All Languages
        /// </remarks>
        /// <response code="200">Returns all languages data if code was not empty</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [HttpGet]
        public async Task<ApiResponse> GetLanguagesAsync([FromQuery] GetAllLanguageKeyValueRequest request)
        {
            var result = await Mediator.Send(new GetAllLanguageKeyValueQuery(request));

            return new ApiResponse(result.Response);
        }

        /// <summary>
        /// Set language key value
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Permission("language_edit")]
        public async Task<ApiResponse> SetKeyValueAsync([FromBody] SetLanguageKeyValueRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var response = await Mediator.Send(new SetLanguageKeyValueCommand(request));
            return new ApiResponse(response);
        }

        /// <summary>
        /// Delete language key value
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Permission("language_edit")]
        public async Task<ApiResponse> DeleteKeyAsync([FromBody] DeleteLanguageKeyValueRequest request)
        {
            await Mediator.Send(new DeleteLanguageKeyValueCommand(request));
            return new ApiResponse();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Permission("language_edit")]
        [HttpPost("restore-cache")]
        public async Task<ApiResponse> RestoreCache()
        {
            await _translationService.SetCacheAsync();
            return new ApiResponse();
        }
    }
}
