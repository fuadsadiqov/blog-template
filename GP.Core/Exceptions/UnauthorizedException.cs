using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using GP.Core.Models;

namespace GP.Core.Exceptions
{

    public class UnauthorizedException : Exception
    {
        public  readonly int statusCode  = StatusCodes.Status401Unauthorized;
        public readonly string errorCode ="UNAUTHORIZED";
        public string title;


        public UnauthorizedException(string title = "Unauthorized access!!")
        {
            this.title = title;
        }
    }

    public class UnauthorizedExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedException unauthorizedException)
            {
                context.Result = new NotFoundObjectResult(new ExceptionResponse()
                {
                    Title = unauthorizedException.title,
                    ErrorCode = unauthorizedException.errorCode,
                })
                {
                    StatusCode = unauthorizedException.statusCode,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
