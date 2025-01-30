using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace GP.Infrastructure.Middlewares
{
    public class UserJwtValidatorsMiddleware
    {
        private readonly RequestDelegate _next;

        public UserJwtValidatorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AuthService authService)
        {
            if (context.User.Identity is { IsAuthenticated: true })
            {

                var tokenClaim = authService.GetTokenClaim();
                var applicationClaimValue = tokenClaim.Application?.Value;
                var rememberMe = tokenClaim.RememberMe?.Value;
                if (string.IsNullOrEmpty(applicationClaimValue) ||
                    string.IsNullOrEmpty(rememberMe))
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }
}
