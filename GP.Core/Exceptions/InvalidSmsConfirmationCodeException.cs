using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace GP.Core.Exceptions
{
    public class InvalidSmsConfirmationCodeException : ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;

        public InvalidSmsConfirmationCodeException(string title = "Confirmation code is not valid.", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
}
