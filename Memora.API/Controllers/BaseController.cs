using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SimpleAUTH.Controllers
{
    /// <summary>
    /// Gets userID from the JWT token
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        protected int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
