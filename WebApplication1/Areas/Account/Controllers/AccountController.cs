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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using GP.Application.Commands.AccountCommands.SignInUser;
using AutoWrapper.Wrappers;
using GP.Application.Commands.UserCommands.CreateUser;
using GP.Infrastructure.Services;

namespace GP.MVC.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly SignInService _signInService;
        private readonly ExceptionService _exceptionService;
        
        public AccountController(ILogger<AccountController> logger, IUserRepository userRepository, SignInService signInService, ExceptionService exceptionService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _signInService = signInService;
            _exceptionService = exceptionService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var email = loginRequest.Email;
                var password = loginRequest.Password;
                SignInUserRequest request = new() { EmailOrUsername = email, Password = password, Code = String.Empty, RememberMe =false };
                var response = await Mediator.Send(new SignInUserCommand(request));
                return RedirectToAction("Index", "Home", new { Area = "Home" });
            }
        
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                CreateUserRequest request = new()
                {
                    Email = registerRequest.Email, 
                    FullNameAz = registerRequest.FullNameAz,
                    FullNameEn = registerRequest.FullNameAz,
                    UserName = registerRequest.Email,
                    Password = registerRequest.Password, 
                    ConfirmPassword = registerRequest.ConfirmPassword,
                };
                if (registerRequest.Password != registerRequest.ConfirmPassword)
                {
                    TempData["ErrorMessage"] = "Şifrələr eyni deyil";
                    return View(registerRequest);
                }
                var response = await Mediator.Send(new CreateUserCommand(request));
                if (response.Response != null)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Home" });
                }
            }
            var message = string.Join(". ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            
            TempData["ErrorMessage"] = message;
            return View(registerRequest);
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
            await _signInService.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "Home" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
