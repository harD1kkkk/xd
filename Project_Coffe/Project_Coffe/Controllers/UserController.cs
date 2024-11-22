using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly AuthenticationService _authService;

        public UserController(UserService userService, AuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userService.GetUserByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest("User already exists.");

            var passwordHash = HashPassword(model.Password);
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = passwordHash,
                Role = "User"
            };

            await _userService.CreateUserAsync(user);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            var token = _authService.GenerateToken(user.Id, user.Role);
            return Ok(new { Token = token });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}

