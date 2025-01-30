using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace GP.Core.Exceptions
{
    public class AccessDeniedException : ApiException
    {
        private const int Statuscode = StatusCodes.Status400BadRequest;

        public AccessDeniedException(string title = "Access denied!", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
}
