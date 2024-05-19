using System.Security.Claims;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewModels;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserModel, CancellationToken ct)
    {
        var user = await _loginService.LoginUser(loginUserModel, ct);
        if (user is null)
        {
            return RedirectToAction("Login");
        }

        await Authenticate(user);

        return Redirect(loginUserModel.ReturnUrl);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserViewModel viewModel, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = await _loginService.RegisterUser(new RegisterUserDto
        {
            Fullname = viewModel.Fullname,
            Email = viewModel.Email,
            Password = viewModel.Password,
        }, ct);

        if (user is null)
        {
            return View(viewModel);
        }

        await Authenticate(user);

        return Redirect(viewModel.ReturnUrl);
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