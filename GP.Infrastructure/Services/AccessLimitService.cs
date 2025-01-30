using GP.Domain.Entities.Identity;

namespace GP.Infrastructure.Services
{
    public class AccessLimitService
    {
        private readonly AuthService _authService;
        private readonly ExceptionService _exceptionService;

        public AccessLimitService(AuthService authService, ExceptionService exceptionService)
        {
            _authService = authService;
            _exceptionService = exceptionService;
        }

        public async Task<bool> CheckAndHideUserDetailAsync(User user)
        {
            var canAccess = false;
            if (user != null)
            {
                var isAdmin = await _authService.IsAdminAsync();
                if (isAdmin)
                    return true;

                var authUserId = _authService.GetAuthorizedUserId();
                var userId = user.Id;

                if (authUserId == userId)
                    return true;
            }

            return canAccess;
        }

        public bool CheckCanAccessUserOutOfDomain(User user, bool throwError = true)
        {
            var isLocalIpAddress = _authService.IsUserIpAddressLocal();
            var canAccessOutOfDomain = user.CanAccessOutOfDomain;
            var canAccess = isLocalIpAddress || canAccessOutOfDomain;

            if (canAccess) return true;
            if (throwError)
                throw _exceptionService.AccessDeniedException();

            return false;
        }
    }
}
