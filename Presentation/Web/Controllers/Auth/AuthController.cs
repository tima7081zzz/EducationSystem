using System.Security.Claims;
using Auth.Models;
using Auth.Services;
using Core.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewModels;

namespace Web.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;

    public AuthController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserModel, CancellationToken ct)
    { 
        var user = await _loginService.LoginUser(loginUserModel, ct);
        if (user is null)
        {
            return RedirectToAction("Login", "Auth");
        }

        await Authenticate(user);

        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserViewModel viewModel, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(viewModel);
            }

            var user = await _loginService.RegisterUser(new RegisterUserDto
            {
                Fullname = viewModel.Fullname,
                Email = viewModel.Email,
                Password = viewModel.Password,
            }, ct);

            if (user is null)
            {
                return BadRequest(viewModel);
            }

            await Authenticate(user);

            return Ok();
        }
        catch (AlreadyExistsException)
        {
            return Conflict();
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    private async Task Authenticate(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        

        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}