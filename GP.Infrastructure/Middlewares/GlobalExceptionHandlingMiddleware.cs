using GP.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NToastNotify;

namespace GP.Infrastructure.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        // private readonly IToastNotification _toastNotification;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _next = next;
            _logger = logger;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            // _toastNotification = toastNotification;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception) 
        {
            context.Items["ErrorMessage"] = exception.Message;

            var controller = context.Request.RouteValues["controller"]!.ToString();
            var action = context.Request.RouteValues["action"]!.ToString();
            var area = context.Request.RouteValues["area"]?.ToString();

            // _toastNotification.AddErrorToastMessage(exception.Message);

            if (exception is RecordNotFoundException)
            {
                action = "Index";
            }

            if (area != null)
            {
                if(area.ToLower() == "Account".ToLower() || area.ToLower() == "Home".ToLower())
                {
                    context.Response.Redirect($"/{controller}/{action}");
                }
                else
                {
                    context.Response.Redirect($"/{area}/{controller}/{action}");
                }
            }
            else
            {
                context.Response.Redirect($"/{controller}/{action}");
            }

            return Task.CompletedTask;
        }
    }
}
