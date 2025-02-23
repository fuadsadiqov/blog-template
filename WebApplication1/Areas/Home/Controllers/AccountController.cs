using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace GP.MVC.Areas.Home.Controllers;

[Area("Home")]
public class AccountController : BaseController
{
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> GoogleResponse()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return RedirectToAction("Login");

        var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
        var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        // Here you can store the user in your database if necessary

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}