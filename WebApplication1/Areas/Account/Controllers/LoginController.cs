using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GP.MVC.Areas.Account.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using GP.Application.Commands.AccountCommands.SignInUserWithGoogle;
using GP.DataAccess.Repository.UserRepository;
using MediatR;

namespace GP.MVC.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserRepository _userRepository;

        public LoginController(ILogger<LoginController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Login", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                var error = authenticateResult.Failure?.Message ?? "Unknown authentication error.";
                Console.WriteLine($"Authentication failed: {error}");
                return RedirectToAction("Detail");
            }

            var claims = authenticateResult.Principal.Identities
                .FirstOrDefault()?.Claims;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var fullname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var s2 = User.Identity.IsAuthenticated;
            SignInUserWithGoogleRequest request = new() { Email = email, FullName = fullname };
            var response = await Mediator.Send(new SignInUserWithGoogleCommand(request));
           var s= User.Identity.IsAuthenticated;
            if (response.IsSigned)
                return RedirectToAction("Index", "Home", new { area = "Home" });
            else
                return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Detail", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
