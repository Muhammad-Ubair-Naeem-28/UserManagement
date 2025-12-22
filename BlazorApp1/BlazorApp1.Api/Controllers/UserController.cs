using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using BlazorApp1.Api.Security;
using System.Security.Claims;
using BlazorApp1.Shared.Dtos;
using BlazorApp1.Interfaces;
using System.Text;

namespace BlazorApp1.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
  
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<TokenResponseDto> LoginUser([FromBody] LoginData loginData)
        {
            var user = _userService.GetUserByEmail(loginData.Email);
            if (user == null || !PasswordHasher.VerifyPassword(loginData.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }

            var expiry = DateTime.UtcNow.AddHours(1);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(user.Role) ? "User" : user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new TokenResponseDto
            {
                Token = tokenString,
                Expiry = expiry,
                User = new UserDto
                {
                    Name = user.Name,
                    Email = user.Email
                }
            });
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<FormData> Me()
        {
            var email = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound($"User with email {email} not found");
            }

            return Ok(user);
        }

    }
}

