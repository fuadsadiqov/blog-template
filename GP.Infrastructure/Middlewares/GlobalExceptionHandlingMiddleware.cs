﻿using GP.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace GP.Infrastructure.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _next = next;
            _logger = logger;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
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

            ITempDataDictionary tempData = _tempDataDictionaryFactory.GetTempData(context);
            tempData["ErrorMessage"] = context.Items["ErrorMessage"];
            tempData.Save();

            if (exception is RecordNotFoundException)
            {
                action = "Index";
            }

            if (area != null)
            {
                if(area.ToLower() == "Account".ToLower())
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
