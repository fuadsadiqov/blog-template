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
    public class UserSignOutResult
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

            await _signInManager.SignInAsync(user, isPersistent: false,
                CookieAuthenticationDefaults.AuthenticationScheme);

            return new UserSignInResult()
            {
                Succeeded = result.Succeeded,
                IsLockOut = result.IsLockedOut
            };

        }
         public async Task<UserSignInResult> SignInWithGoogleAsync(User user)
        {
            
            await _signInManager.SignInAsync(user, isPersistent: false);
            
            return new UserSignInResult()
            {
                Succeeded = true,
                IsLockOut = false
            };

        }

        public async Task<UserSignOutResult> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return new UserSignOutResult()
            {
                Succeeded = true,
                IsLockOut = false
            };
        }
    }
}
