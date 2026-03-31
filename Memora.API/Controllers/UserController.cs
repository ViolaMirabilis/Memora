using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;
using SimpleAUTH.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SimpleAUTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            // Gets all the users from the actual database
            var users = await _userService.GetAllUsers();

            // returns a mapped list of users with only the Nickname property
            var result = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Nickname = u.Nickname,
                LastLoginAt = u.LastLoginAt
            }).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            // getting a user form the database by id (containing all the details)
            var user = await _userService.GetUserById(id);

            if (user == null)
                return NotFound();

            // creating a dto which is delivered to the client with only the Nickname property
            var dto = new UserDTO
            {
                Id = user.Id,
                Nickname = user.Nickname,
                LastLoginAt = user.LastLoginAt
            };

            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            return Ok(_userService.CreateUser(user));
        }


        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<User> UpdateUser(int id, User updatedUser)
        {
            var result = _userService.UpdateUser(id, updatedUser);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
