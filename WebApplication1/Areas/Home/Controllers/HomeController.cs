using GP.Application.BlogQueries.GetAllBlogsQuery;
using GP.Application.BlogQueries.GetBlogQuery;
using GP.MVC.Areas.Home.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GP.Application.Commands.BlogCommands.UpdateBlogViewCount;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using GP.DataAccess.Repository.UserRepository;

namespace GP.MVC.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await Mediator.Send(new GetAllBlogsQuery(new GetAllBlogsRequest()));
            var groupedBlogs = blogs.BlogResponses.GroupBy(b => b.Category.Title)
                                                   .ToDictionary(g => g.Key, g => g.ToList());
            
            return View(groupedBlogs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail( Guid id)
        {
            UpdateBlogViewCountRequest updateRequest = new UpdateBlogViewCountRequest{ Id = id };
            await Mediator.Send(new UpdateBlogViewCountCommand(updateRequest));
            
            GetBlogRequest request = new GetBlogRequest{ Id = id };
            var blog = await Mediator.Send(new GetBlogQuery(request));
            var lastBlogs = await Mediator.Send(new GetAllBlogsQuery(new GetAllBlogsRequest()));

            BlogDetailViewModel model = new BlogDetailViewModel{ lastBlogs = lastBlogs.BlogResponses, blog = blog.BlogResponse };
            
            return View(model);
        }

        public IActionResult Login()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Home", null, Request.Scheme);
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
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null)
                return RedirectToAction("Detail");

            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                //user = await _userRepository.CreateAsync(email, name);
            }

            // Sign in the user
            var claimsIdentity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        }, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Detail", "Home"); // Redirect to homepage after login
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Detail", "Home");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
