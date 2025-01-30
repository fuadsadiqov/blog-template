using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using GP.Application.Commands.UserCommands.ChangeUserPassword;
using GP.Application.Commands.UserCommands.CreateUser;
using GP.Application.Commands.UserCommands.DeleteUser;
using GP.Application.Commands.UserCommands.LockUser;
using GP.Application.Commands.UserCommands.ResetPassword;
using GP.Application.Commands.UserCommands.SetUserStatus;
using GP.Application.Commands.UserCommands.StartPasswordRestoration;
using GP.Application.Commands.UserCommands.UnLockUser;
using GP.Application.Commands.UserCommands.UpdateUser;
using GP.Application.Queries.UserQueries;
using GP.Application.Queries.UserQueries.GetAllUser;
using GP.Application.Queries.UserQueries.GetUser;
using GP.Core.Models;
using GP.Infrastructure.Middlewares;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.API.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : BaseController
    {
        private readonly AuthService _authService;

        public UserController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Get User Data
        /// </summary>
        /// <remarks>
        /// Gets User Data by id: `id`
        /// </remarks>
        /// <response code="200">Returns user data</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<GetUserResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetAsync(string id)
        {
            var result = await Mediator.Send(new GetUserQuery(new GetUserRequest() { Id = id }));
            return new ApiResponse(result.Response);
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <remarks>
        /// Gets data about All Users
        /// </remarks>
        /// <response code="200">Returns List of User datas</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<GetAllUserResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<ApiResponse> GetAllAsync([FromQuery] UserFilterParameters filterParameters,
            [FromQuery] List<SortParameters> sortParameters, [FromQuery] PagingParameters pagingParameters)
        {
            var result = await Mediator.Send(new GetAllUserQuery(new GetAllUserRequest()
            {
                FilterParameters = filterParameters,
                PagingParameters = pagingParameters,
                SortParameters = sortParameters
            }));
            // return result;
            return new ApiResponse(result.Response);
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <remarks>
        /// Creates User
        /// </remarks>
        /// <response code="200">Returns `id` of created user</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<string>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("user_add")]
        [HttpPost]
        public async Task<ApiResponse> CreateAsync([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var result = await Mediator.Send(new CreateUserCommand(request));

            return new ApiResponse(await GetAsync(result.Response.Id));
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <remarks>
        /// Updates User data
        /// </remarks>
        /// <response code="200">Returns `id` of updated user</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<string>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("user_edit")]
        [HttpPut]
        public async Task<ApiResponse> UpdateAsync([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var result = await Mediator.Send(new UpdateUserCommand(request));

            return new ApiResponse(await GetAsync(result.Response.Id));
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <remarks>
        /// Changes Password of user with id: `id`
        /// </remarks>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("user_edit")]
        [HttpPut("{id}/change-password")]
        public async Task<ApiResponse> ChangePassword(string id, [FromBody] ChangeUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            await Mediator.Send(new ChangeUserPasswordCommand(id, false, request));

            return new ApiResponse(result: null);
        }

        /// <summary>
        /// Get Default Options
        /// </summary>
        /// <remarks>
        /// Gets Default Options for User
        /// </remarks>
        /// <response code="200">Returns Default Options</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [HttpPut("change-password")]
        public async Task<ApiResponse> ChangePassword([FromBody] ChangeUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var authUserId = _authService.GetAuthorizedUserId();

            await Mediator.Send(new ChangeUserPasswordCommand(authUserId, false, request));

            return new ApiResponse(result: null);
        }

        /// <summary>
        /// Update Status
        /// </summary>
        /// <remarks>
        /// Updates Status of user with id: `id`
        /// </remarks>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("admin", "user_edit")]
        [HttpPut("{id}/set-status")]
        public async Task<ApiResponse> SetUserStatusAsync(string id, [FromBody] SetUserStatusRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            request.UserId = id;

            await Mediator.Send(new SetUserStatusCommand(request));

            return new ApiResponse(result: null);
        }

        /// <summary>
        /// Get Default Options
        /// </summary>
        /// <remarks>
        /// Gets Default Options for User
        /// </remarks>
        /// <response code="200">Returns Default Options</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("user_delete")]
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(string id)
        {
            await Mediator.Send(new DeleteUserCommand(new DeleteUserRequest()
            {
                UserId = id
            }));
            return new ApiResponse(result: null);
        }

        /// <summary>
        /// Get Default Options
        /// </summary>
        /// <remarks>
        /// Gets Default Options for User
        /// </remarks>
        /// <response code="200">Returns Default Options</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("user_edit")]
        [HttpPost("Lock")]
        public async Task<ApiResponse> LockUser([FromBody] LockUserRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            await Mediator.Send(new LockUserCommand(request));

            return new ApiResponse();
        }

        /// <summary>
        /// Get Default Options
        /// </summary>
        /// <remarks>
        /// Gets Default Options for User
        /// </remarks>
        /// <response code="200">Returns Default Options</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("user_edit")]
        [HttpPost("UnLock")]
        public async Task<ApiResponse> UnLockUser([FromBody] UnLockUserRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            await Mediator.Send(new UnLockUserCommand(request));

            return new ApiResponse();
        }












        /*[Produces("application/json")]
        [HttpPost]
        public async Task<CreateUserResponse> CreateAsync([FromBody] CreateUserRequest request)
        {
            var result = await Mediator.Send(new CreateUserCommand(request));
            return result;
        }

        [Produces("application/json")]
        [HttpPut]
        public async Task<UpdateUserResponse> UpdateAsync([FromBody] UpdateUserRequest request)
        {
            var result = await Mediator.Send(new UpdateUserCommand(request));
            return result;
        }

        [Produces("application/json")]
        [HttpDelete]
        public async Task<DeleteUserResponse> DeleteAsync([FromBody] DeleteUserRequest request)
        {
            var result = await Mediator.Send(new DeleteUserCommand(request));
            return result;
        }

        [Produces("application/json")]
        [HttpPut("passive")]
        public async Task<SetPassiveUserResponse> SetPassiveUserAsync([FromBody] SetPassiveUserRequest request)
        {
            var result = await Mediator.Send(new SetPassiveUserCommand(request));
            return result;
        }*/

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("request-password-change")]
        public async Task<StartPasswordRestorationResponse> RequestPasswordChangeAsync([FromBody] StartPasswordRestorationRequest request)
        {
            var result = await Mediator.Send(new StartPasswordRestorationCommand(request));
            return result;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("reset-password")]
        public async Task<ResetPasswordResponse> ConfirmAsync([FromBody] ResetPasswordRequest request)
        {
            var result = await Mediator.Send(new ResetPasswordCommand(request));
            return result;
        }
    }
}
