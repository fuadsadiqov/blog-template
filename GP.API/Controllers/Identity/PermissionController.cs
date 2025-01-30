using AutoWrapper.Wrappers;
using GP.Application.Queries.PermissionQueries.GetAllDirectivePermission;
using GP.Application.Queries.PermissionQueries.GetAllPermission;
using GP.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.API.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PermissionController : BaseController
    {

        /// <summary>
        /// Permissions
        /// </summary>
        /// <remarks>
        /// Gets all permissions
        /// </remarks>
        /// <response code="200">Gets  all permissions </response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<GetAllPermissionResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [HttpGet]
        public async Task<ApiResponse> GetAllAsync()
        {
            var result = await Mediator.Send(new GetAllPermissionQuery());

            return new ApiResponse(result.Response);
        }

        /// <summary>
        /// Directive Permissions
        /// </summary>
        /// <remarks>
        /// Gets all directive permissions
        /// </remarks>
        /// <response code="200">Gets  all directive permissions </response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<GetAllDirectivePermissionResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [HttpGet("directive-permissions")]
        public async Task<ApiResponse> GetAllDirectivePermissionsAsync()
        {
            var result = await Mediator.Send(new GetAllDirectivePermissionQuery());

            return new ApiResponse(result.Response);
        }
    }
}
