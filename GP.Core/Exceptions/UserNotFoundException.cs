using GP.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GP.Core.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public readonly int statusCode = StatusCodes.Status404NotFound;
        public readonly string errorCode = "NOT_FOUND";
        public string title;

        public UserNotFoundException(string title = "Requested data not found.")
        {
            this.title = title;
        }
    }

    public class UserNotFoundExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UserNotFoundException userNotFoundException)
            {
                context.Result = new NotFoundObjectResult(new ExceptionResponse()
                {
                    Title = userNotFoundException.title,
                    ErrorCode = userNotFoundException.errorCode,
                })
                {
                    StatusCode = userNotFoundException.statusCode,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
