using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using GP.Core.Models;

namespace GP.Core.Exceptions
{

    public class RecordAlreadyExistsException : Exception
    {
        public  readonly int statusCode  = StatusCodes.Status409Conflict;
        public readonly string errorCode = "RECORD_ALREADY_EXISTS";
        public string title;


        public RecordAlreadyExistsException(string title = "Record already exists!!")
        {
            this.title = title;
        }
    }

    public class RecordAlreadyExistsExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RecordAlreadyExistsException recordAlreadyExistsException)
            {
                context.Result = new NotFoundObjectResult(new ExceptionResponse()
                {
                    Title = recordAlreadyExistsException.title,
                    ErrorCode = recordAlreadyExistsException.errorCode,
                })
                {
                    StatusCode = recordAlreadyExistsException.statusCode,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
