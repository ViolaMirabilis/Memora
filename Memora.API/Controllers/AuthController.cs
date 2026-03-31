using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleAUTH.DTO;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAUTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : BaseController
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest request)
        {
            var user = await authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("Username already exists!");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            // .Result ensures that the return type is a string, not a Task<string>
            var token = await authService.LoginAsync(request);

            if(token == null)
            {
                return BadRequest("Something ventrom");
            }
            //return Ok(token);         // returns plain token string
            return Ok(new LoginResponse { Token = token });     // return login response object, {Token = asdjkasd....}
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnly()
        {
            return Ok("You are authenticated");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are authenticated");
        }
    }
}
