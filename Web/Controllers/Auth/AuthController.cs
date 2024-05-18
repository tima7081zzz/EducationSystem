using System.Security.Claims;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Auth;

public class AuthController : BaseController
{
    private readonly ILoginService _loginService;

    public AuthController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginUserDto loginUserModel, CancellationToken ct)
    {
        var user = await _loginService.GetUser(loginUserModel, ct);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        await Authenticate(user);

        return Ok();
    }

    private async Task Authenticate(UserDto user)
    {
        //var userRole = UserRoles.ResolveByEnumValue(user.RoleType);

        var claims = new List<Claim>
        {
            //new(ClaimsIdentity.DefaultRoleClaimType, userRole),
            new(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        

        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}