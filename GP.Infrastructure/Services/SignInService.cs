using GP.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using GP.Core.Models;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.UserRepository;

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
        public SignInService(SignInManager<User> signInManager, IUnitOfWork unitOfWork, UserManager<User> userManager, IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userRepository = userRepository;

        }

        public async Task<UserSignInResult> LocalSignInAsync(User user, string password)
        {
            var dateTimeNow = DateTime.Now.AddHours(-1);
            if (user.LastAccessFailedAttempt < dateTimeNow)//unlock user if 1 hour has passed since last failed access attempt
            {
                await _userRepository.SetUnLockoutAsync(user);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut) user.LastAccessFailedAttempt = null;
                else user.LastAccessFailedAttempt = DateTime.Now;
                await _unitOfWork.CompleteAsync();
            }

            return new UserSignInResult()
            {
                Succeeded = result.Succeeded,
                IsLockOut = result.IsLockedOut
            };
        }

        /*public async Task<UserSignInResult> LdapSignInAsync(string userName, string password)
        {
            using var cn = new LdapConnection();

            cn.Connect(_ldapSettings.Host, _ldapSettings.Port);
            try
            {
                var username = $"{_ldapSettings.UserNamePrefix}{userName}";
                await cn.BindAsync(Native.LdapAuthType.Simple, new LdapCredential
                {
                    UserName = username,
                    Password = password
                });
                return new UserSignInResult()
                {
                    Succeeded = true,

                };
            }
            catch (Exception e)
            {
                if (e is LdapInvalidCredentialsException)
                {
                    return new UserSignInResult()
                    {
                        Succeeded = false,
                        Exception = e,

                    };
                }
                else
                    throw;
            }
        }*/
    }
}
