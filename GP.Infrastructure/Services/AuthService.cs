using GP.Core.Constants;
using GP.Core.Enums.Enitity;
using GP.Core.Extensions;
using GP.Core.Models;
using GP.Core.Utilities;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GP.Infrastructure.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LocalIpAddressSettings _localIpAddressSettings;

        public AuthService(IMediator mediator,
            IUserRepository userRepository,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOptions<LocalIpAddressSettings> localIpAddressSettings)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _localIpAddressSettings = localIpAddressSettings.Value;
        }

        public string GetAuthorizedUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        /*public bool IsSuperAdminAsync()
        {
            var userId = GetAuthorizedUserId();
            var superAdminRoleId = _roleRepository.FindBy(x => x.NormalizedName == "SUPERADMIN").ToList().FirstOrDefault()?.Id;
            if (userId == null)
            {
                throw new InvalidOperationException();
            }
            var result = _userRepository.FindBy(x => x.Id == userId).Any(x => x.Roles.Any(y => y.RoleId == superAdminRoleId));
            return result;
        }

        public bool IsAdminAsync()
        {
            var userId = GetAuthorizedUserId();
            var adminRoleId = _roleRepository.FindBy(x => x.NormalizedName == "ADMIN").ToList().FirstOrDefault()?.Id;
            if (userId == null)
            {
                throw new InvalidOperationException();
            }
            var result = _userRepository.FindBy(x => x.Id == userId).Any(x => x.Roles.Any(y => y.RoleId == adminRoleId));
            return result;
        }*/

        public List<Claim> GetClaims()
        {
            var claims = _httpContextAccessor.HttpContext?.User.Claims.ToList();
            return claims;
        }

        public TokenClaim GetTokenClaim()
        {
            var claims = GetClaims();
            return new TokenClaim()
            {
                Application = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Application),
                Domain = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Domain),
                ImpersonatorId = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Impersonator),
                ImpersonatorName = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.ImpersonatorName),
                CanAccessOutOfDomain =
                    claims.FirstOrDefault(c => c.Type == CustomClaimTypes.CanAccessOutOfDomain),
                RememberMe =
                    claims.FirstOrDefault(c => c.Type == CustomClaimTypes.RememberMe)
            };
        }

        public async Task<bool> IsAdminAsync()
        {
            var userId = GetAuthorizedUserId();
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            return await UserIsInPermissionAsync(userId, nameof(PermissionEnum.Admin));
        }

        public async Task<bool> IsAdminAsync(string userId)

        {
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            return await UserIsInPermissionAsync(userId, nameof(PermissionEnum.Admin));
        }

        public bool IsUserIpAddressLocal()
        {
            var currentIpAddress = _httpContextAccessor.HttpContext?.GetRequestIp();

            if (string.IsNullOrEmpty(currentIpAddress)) return false;

            var lanIpAddressUtil = new IpAddressUtil(_localIpAddressSettings.LanIpAddressPattern);
            var wifiIpAddressUtil = new IpAddressUtil(_localIpAddressSettings.WiFiIpAddressPattern);

            var publicIp = "178.76.0.20";
            var publicSubIp = "192.168.90.";

            return (lanIpAddressUtil.IsLocalIpAddress(currentIpAddress) ||
                    wifiIpAddressUtil.IsLocalIpAddress(currentIpAddress) ||
                    publicIp == currentIpAddress ||
                    currentIpAddress.StartsWith(publicSubIp));
        }

        public async Task<bool> UserIsInRoleAsync(User user, string roleName)
        {
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            return roles.Any(s => s.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> UserIsInPermissionAsync(string userId, string permissionName)
        {
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            var user = await _userRepository.GetUserByIdAsync(userId, includeParams.ToArray()).ConfigureAwait(false);
            var directivePermissions = user.DirectivePermissions.Select(c => c.Permission.Label).ToList();
            var userRole = user.Roles.Select(c => c.Role).ToList();
            var permissions = userRole.SelectMany(c => c.PermissionCategory.Select(e => $"{e.PermissionCategoryPermission.Category.Label.ToLower()}_{e.PermissionCategoryPermission.Permission.Label.ToLower()}")).ToList();

            return directivePermissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase)) ||
                   permissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
