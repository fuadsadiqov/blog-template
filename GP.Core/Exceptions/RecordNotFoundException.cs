using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace GP.Core.Exceptions
{
    public class RecordNotFoundException : ApiException
    {
        private const int Statuscode = StatusCodes.Status404NotFound;

        public RecordNotFoundException(string title = "Requested data not found.", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
    /*public class RecordNotFoundException : Exception
    {
        public  readonly int statusCode  = StatusCodes.Status404NotFound;
        public readonly string errorCode ="NOT_FOUND";
        public string title;


        public RecordNotFoundException(string title = "Requested data not found.")
        {
            this.title = title;
        }
    }

    public class RecordNotFoundExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RecordNotFoundException notFoundException)
            {
                context.Result = new NotFoundObjectResult(new ExceptionResponse()
                {
                    Title = notFoundException.title,
                    ErrorCode = notFoundException.errorCode,
                })
                {
                    StatusCode = notFoundException.statusCode,
                };
                context.ExceptionHandled = true;
            }
        }
    }*/
}
