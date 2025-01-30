using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using GP.Application.Commands.AccountCommands.ForgotPassword;
using GP.Application.Commands.AccountCommands.RefreshToken;
using GP.Application.Commands.AccountCommands.SignInUser;
using GP.Application.Commands.AccountCommands.SignOutUser;
using GP.Application.Commands.AccountCommands.StartImpersonate;
using GP.Application.Commands.AccountCommands.StopImpersonate;
using GP.Application.Queries.UserQueries.GetAuthUser;
using GP.Core.Extensions;
using GP.Core.Models;
using GP.Infrastructure.Middlewares;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// User Sign In
        /// </summary>
        /// <remarks>
        /// In case if `code` is not empty and  valid then `Access` and `Refresh` token being created. 
        /// </remarks>
        /// <response code="200">Returns Access and Refresh token for the user if code was not empty</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<SignInUserResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [HttpPost("Auth")]
        public async Task<ApiResponse> AuthAsync([FromBody] SignInUserRequest request)
        {
            var isEmail = request.EmailOrUsername.IsEmail();
            var isUsername = request.EmailOrUsername.IsUsername();

            if (!(isEmail || isUsername))
                ModelState.AddModelError("emailOrUsername", "Enter valid Email/Username");

            var response = await Mediator.Send(new SignInUserCommand(request));

            return new ApiResponse(response);
        }

        /// <summary>
        /// Password Reset
        /// </summary>
        /// <remarks>
        /// `LDAP` user cannot change his password 
        /// </remarks>
        /// <response code="200">Returns Access and Refresh token for the user if code was not empty</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<ForgotPasswordResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [HttpPost("PasswordReset")]
        public async Task<ApiResponse> PasswordResetAsync([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var isEmail = request.EmailOrUsername.IsEmail();
            var isUsername = request.EmailOrUsername.IsUsername();

            if (!(isEmail || isUsername))
                ModelState.AddModelError("emailOrUsername", "Enter valid Email/Username");

            var response = await Mediator.Send(new ForgotPasswordCommand(request));

            return new ApiResponse(result: response);
        }

        /// <summary>
        /// Get Authorized User
        /// </summary>
        /// <remarks>
        /// Gets infomrmation about authorized user
        /// </remarks>
        /// <response code="200">Returns information about the user</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<GetAuthUserResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetAuthorizedUser")]
        public async Task<ApiResponse> GetAuthorizedAsync()
        {
            var result = await Mediator.Send(new GetAuthUserQuery(new GetAuthUserRequest()));

            return new ApiResponse(result.Response);
        }

        /// <summary>
        /// User Sign Out
        /// </summary>
        /// <remarks>
        /// `Access` and `Refresh` token will be invalidated. 
        /// </remarks>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<SignOutUserResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("SignOut")]
        public async Task<ApiResponse> SignOutAsync()
        {
            var response = await Mediator.Send(new SignOutUserCommand(new SignOutUserRequest()));

            return new ApiResponse(response);
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <remarks>
        /// Refreshing `Refresh` and `Access` tokens
        /// </remarks>
        /// <response code="200">Returns new Access and Refresh tokens</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<RefreshTokenResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [HttpGet("refresh-token")]
        public async Task<ApiResponse> GetToken()
        {
            var userId = _authService.GetAuthorizedUserId();
            var response = await Mediator.Send(new RefreshTokenCommand(new RefreshTokenRequest()
            {
                UserId = userId
            }));

            return new ApiResponse(response);
        }

        /// <summary>
        /// Start Impersonate
        /// </summary>
        /// <remarks>
        /// Starts `Impersonate` mode for user (with admin role), where `Impersonate` means entering to any user account and using it without credentials.
        /// </remarks>
        /// <response code="200">Returns Access and Refresh token for the user</response>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<StartImpersonateResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Permission("admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("start-impersonate")]
        public async Task<ApiResponse> StartImpersonate(string userId)
        {
            var result = (await Mediator.Send(new StartImpersonateCommand(new StartImpersonateRequest()
            {
                UserId = userId
            })));

            return new ApiResponse(result: result);
        }

        /// <summary>
        /// Stop Impersonate
        /// </summary>
        /// <remarks>
        /// Stops `Impersonate` mode for user (with admin role), where `Impersonate` means entering to any user account and using it without credentials.
        /// </remarks>
        /// <response code="400">Validation error</response>
        /// <response code="500">Server Error</response>
        [ProducesResponseType(typeof(DefaultApiSchemaResponse<StopImpersonateResponse>), 200)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 400)]
        // [ProducesResponseType(typeof(DefaultApiSchemaResponse<ApiException>), 500)]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("stop-impersonate")]
        public async Task<ApiResponse> StopImpersonate()
        {
            await Mediator.Send(new StopImpersonateCommand(new StopImpersonateRequest()
            {
            }));
            return new ApiResponse(result: null);
        }
    }
}
