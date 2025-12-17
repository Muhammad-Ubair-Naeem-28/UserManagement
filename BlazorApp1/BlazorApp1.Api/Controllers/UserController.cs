using BlazorApp1.Interfaces;
using BlazorApp1.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FormData>> GetUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{email}")]
        public ActionResult<FormData> GetUser(string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null) return NotFound($"User with email {email} not found");
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] FormData userData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _userService.CreateUser(userData);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            return CreatedAtAction(nameof(GetUser), new { email = userData.Email }, userData);
        }

        [HttpPut("{email}")]
        public IActionResult UpdateUser(string email, FormData userData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (email != userData.Email) return BadRequest("Email in URL does not match body");

            if (!_userService.UpdateUser(email, userData)) return NotFound($"User with email {email} not found");

            return NoContent();
        }

        [HttpDelete("{email}")]
        public IActionResult DeleteUser(string email)
        {
            if (!_userService.DeleteUser(email)) return NotFound($"User with email {email} not found");
            return NoContent();
        }
    }
}
