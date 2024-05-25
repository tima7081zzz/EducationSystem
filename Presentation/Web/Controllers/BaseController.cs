using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class BaseController : ControllerBase
{
    internal int UserId
    {
        get
        {
            if (!User.Identity!.IsAuthenticated)
            {
                throw new Exception("Not authenticated"); //todo: change to custom exception
            }
            
            var nameClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(nameClaim, out var userId) == false)
            {
                throw new Exception("Not authenticated"); //todo: change to custom exception
            }

            return userId;
        }
    }
}