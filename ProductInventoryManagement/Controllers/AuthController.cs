using Microsoft.AspNetCore.Mvc;
using ProductInventoryManagement.DTO;
using ProductInventoryManagement.Models;
using ProductInventoryManagement.Services;
using Serilog;
using System.Threading.Tasks;

namespace ProductInventoryManagement.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")] // Specify the version
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var token = await _userService.AuthenticateAsync(loginModel.Username, loginModel.Password);
            if (token == null)
            {
                Log.Error("Unknown user try to login with username: {@Username}", loginModel.Username);
                return Unauthorized();
            }
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Invalid user data.");
            }

            User user = null;
            try
            {

                user = await _userService.RegisterUserAsync(model.Username, model.Password, model.Role);

                Log.Information("User created successfully: {@Username}", model.Username);

                return CreatedAtAction(nameof(Register), new { id = user.UserID }, user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while registering user: {@Username}", model.Username);
                return BadRequest(ex.Message);
            }
        }
    }
}
