using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace GP.Core.Exceptions
{
    public class CorporateUserSignInException : ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;

        public CorporateUserSignInException(string title = "This is corporate user, please contact administrator.", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
}
