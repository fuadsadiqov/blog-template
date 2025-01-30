using GP.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GP.Core.Exceptions
{
    public class RecordInUseException : Exception
    {
        public readonly int statusCode = StatusCodes.Status409Conflict;
        public readonly string errorCode = "CANNOT_BE_DELETED";
        public string title;

        public RecordInUseException(string title = "The record is already in use and cannot be deleted.")
        {
            this.title = title;
        }
    }

    public class RecordInUseExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RecordInUseException recordInUseException)
            {
                context.Result = new NotFoundObjectResult(new ExceptionResponse()
                {
                    Title = recordInUseException.title,
                    ErrorCode = recordInUseException.errorCode,
                })
                {
                    StatusCode = recordInUseException.statusCode,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
