using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Match.Controllers;

public class LoginController(ILogger<LoginController> logger) : Controller
{
    private readonly ILogger<LoginController> _logger = logger;

    public IActionResult GoogleLogin()
    {
        return new ChallengeResult("Google", new()
        {
            RedirectUri = Url.Action("SigninGoogle", "Login"),
        });
    }

    public async Task<IActionResult> SigninGoogle()
    {
        var user = User.Identities.FirstOrDefault();

        if (user?.IsAuthenticated == true)
        {
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new(user),
            new()
            {
                IsPersistent = true,
                RedirectUri = Request.Host.Value
            });
        }

        return LocalRedirect("/");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return LocalRedirect("/");
    }
}
