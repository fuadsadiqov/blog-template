using GP.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace GP.Infrastructure.Services
{
    public class UserSignInResult
    {
        public bool Succeeded { get; set; }
        public bool IsLockOut { get; set; }
        public Exception Exception { get; set; }
    }
    public class SignInService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SignInService(SignInManager<User> signInManager, IUnitOfWork unitOfWork, UserManager<User> userManager, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserSignInResult> LocalSignInAsync(User user, string password)
        {
            var dateTimeNow = DateTime.Now.AddHours(-1);
            if (user.LastAccessFailedAttempt <
                dateTimeNow) //unlock user if 1 hour has passed since last failed access attempt
            {
                await _userRepository.SetUnLockoutAsync(user);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut) user.LastAccessFailedAttempt = null;
                else user.LastAccessFailedAttempt = DateTime.Now;
                await _unitOfWork.CompleteAsync();

                return new UserSignInResult()
                {
                    Succeeded = result.Succeeded,
                    IsLockOut = result.IsLockedOut
                };
            }

            // Create a ClaimsIdentity with user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                // Add additional claims as needed
            };

            // Optionally, you can add roles as claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Explicitly sign the user in using cookies
            await _signInManager.SignInAsync(user, isPersistent: false,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal);

            return new UserSignInResult()
            {
                Succeeded = result.Succeeded,
                IsLockOut = result.IsLockedOut
            };

        }
         public async Task<UserSignInResult> SignInWithGoogleAsync(User user)
        {
            // var dateTimeNow = DateTime.Now.AddHours(-1);
            // if (user.LastAccessFailedAttempt <
            //     dateTimeNow) //unlock user if 1 hour has passed since last failed access attempt
            // {
            //     await _userRepository.SetUnLockoutAsync(user);
            // }

            // Create a ClaimsIdentity with user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                // Add additional claims as needed
            };

            // Optionally, you can add roles as claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Explicitly sign the user in using cookies
            await _signInManager.SignInAsync(user, isPersistent: false,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal);

            return new UserSignInResult()
            {
                Succeeded = true,
                IsLockOut = true
            };

        }
    }
}
