using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using GP.Application.Commands.RoleCommands.CreateRole;
using GP.Application.Commands.RoleCommands.DeleteRole;
using GP.Application.Commands.RoleCommands.UpdateRole;
using GP.Application.Queries.RoleQueries;
using GP.Application.Queries.RoleQueries.GetAllRole;
using GP.Application.Queries.RoleQueries.GetRole;
using GP.Core.Models;
using GP.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.API.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RoleController : BaseController
    {
        /// <summary>
        /// Get Role Description
        /// </summary>
        /// <remarks>
        /// Gets Role Description
        /// </remarks>
        /// <response code="200">Returns role data</response>
        /// <response code="401">User not authorized</response>
        /// <response code="500">Server Error</response>            
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<GetRoleResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 401)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("role_list")]
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetAsync(string id)
        {
            var result = await Mediator.Send(new GetRoleQuery(new GetRoleRequest()
            {
                Id = id
            }));
            return new ApiResponse(result.Response);
        }

        /// <summary>
        /// Get list of All Roles
        /// </summary>
        /// <remarks>
        /// Gets list of All Roles
        /// </remarks>
        /// <response code="200">Returns list of roles</response>
        /// <response code="401">User not authorized</response>
        /// <response code="500">Server Error</response>            
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<GetAllRoleResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 401)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("role_list")]
        [HttpGet]
        public async Task<ApiResponse> GetAllAsync([FromQuery] RoleFilterParameters filterParameters,
            [FromQuery] PagingParameters pagingParameters, [FromQuery] List<SortParameters> sortParameters)
        {
            var result = await Mediator.Send(new GetAllRoleQuery(new GetAllRoleRequest()
            {
                FilterParameters = filterParameters,
                PagingParameters = pagingParameters,
                SortParameters = sortParameters
            }));
            return new ApiResponse(result.Response);
        }

        /// <summary>
        /// Create Role
        /// </summary>
        /// <remarks>
        /// Creates Role
        /// </remarks>
        /// <response code="200">Id of created role</response>
        /// <response code="401">User not authorized</response>
        /// <response code="500">Server Error</response>            
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<string>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 401)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("role_add")]
        [HttpPost]
        public async Task<ApiResponse> CreateAsync([FromBody] CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());
            var result = await Mediator.Send(new CreateRoleCommand(request));

            return new ApiResponse(result: result.Response);
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <remarks>
        /// Updates Role
        /// </remarks>
        /// <response code="200">Id of edited role</response>
        /// <response code="401">User not authorized</response>
        /// <response code="500">Server Error</response>            
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<string>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 401)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("role_edit")]
        [HttpPut]
        public async Task<ApiResponse> UpdateAsync([FromBody] UpdateRoleRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());
            var result = await Mediator.Send(new UpdateRoleCommand(request));

            return new ApiResponse(result: result.Response);
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <remarks>
        /// Deletes Role
        /// </remarks>
        /// <response code="401">User not authorized</response>
        /// <response code="500">Server Error</response>            
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 401)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("role_delete")]
        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteAsync(string id)
        {
            await Mediator.Send(new DeleteRoleCommand(new DeleteRoleRequest()
            {
                Id = id
            }));

            return new ApiResponse(result: null);
        }
    }
}
