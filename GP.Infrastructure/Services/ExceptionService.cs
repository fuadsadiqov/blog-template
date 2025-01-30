using GP.Core.Exceptions;
using GP.Core.Resources;
using Microsoft.Extensions.Localization;

namespace GP.Infrastructure.Services
{
    public class ExceptionService
    {
        private readonly IStringLocalizer<Resource> _localizer;

        public ExceptionService(IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
        }

        public RecordNotFoundException RecordNotFoundException(string key = ResourceKey.RecordNotFound)
        {
            var message = _localizer[key];
            return new RecordNotFoundException(message, key);
        }

        public WrongRequestException WrongRequestException(string message = null)
        {
            const string key = ResourceKey.WrongRequest;

            if (string.IsNullOrEmpty(message))
            {
                message = _localizer[key];
            }

            return new WrongRequestException(message, key);
        }

        public RecordAlreadyExistException RecordAlreadyExistException()
        {
            const string key = ResourceKey.RecordAlreadyExist;
            var message = _localizer[key];
            return new RecordAlreadyExistException(message, key);
        }

        public RecordNotEditableException RecordNotEditableException()
        {
            const string key = ResourceKey.RecordNotEditable;
            var message = _localizer[key];
            return new RecordNotEditableException(message, key);
        }

        public AccessDeniedException AccessDeniedException()
        {
            const string key = ResourceKey.AccessDenied;
            var message = _localizer[key];
            return new AccessDeniedException(message, key);
        }

        public InvalidSmsConfirmationCodeException InvalidSmsConfirmationCodeException()
        {
            const string key = ResourceKey.InvalidSmsConfirmationCode;
            var message = _localizer[key];
            return new InvalidSmsConfirmationCodeException(message, key);
        }

        public InvalidSmsConfirmationCodeException InvalidSmsConfirmationCodeAndResendException()
        {
            const string key = ResourceKey.InvalidSmsConfirmationCodeResend;
            var message = _localizer[key];
            return new InvalidSmsConfirmationCodeException(message, key);
        }

        public InvalidSignInException InvalidSignInException()
        {
            const string key = ResourceKey.InvalidSignIn;
            var message = _localizer[key];
            return new InvalidSignInException(message, key);
        }

        public LockedUserException LockedUserException()
        {
            const string key = ResourceKey.LockedUser;
            var message = _localizer[key];
            return new LockedUserException(message, key);
        }

        public CorporateUserSignInException CorporateUserSignInException()
        {
            const string key = ResourceKey.CorporateUserSignIn;
            var message = _localizer[key];
            return new CorporateUserSignInException(message, key);
        }

        public UnauthorizedException UnauthorizedException()
        {
            return new UnauthorizedException();
        }

        public RecordAlreadyExistsException RecordAlreadyExistsException()
        {
            return new RecordAlreadyExistsException();
        }

        public RecordInUseException RecordInUseException()
        {
            return new RecordInUseException();
        }

        public UserNotFoundException UserNotFoundException()
        {
            return new UserNotFoundException();
        }
    }
}
