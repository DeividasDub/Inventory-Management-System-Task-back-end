using Microsoft.AspNetCore.Mvc;
using InventoryManagementAPI.DTOs.Auth;
using InventoryManagementAPI.Services;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuthModelFactory _authModelFactory;

        public AuthController(
            IAuthService authService, 
            IAuthModelFactory authModelFactory)
        {
            _authService = authService;
            _authModelFactory = authModelFactory;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (user, token) = await _authService.RegisterAsync(request);
            
            if (user == null || token == null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var model = _authModelFactory.PrepareAuthResponseModel(user, token, DateTime.UtcNow.AddHours(24));

            return Ok(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (user, token) = await _authService.LoginAsync(request);
            
            if (user == null || token == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var model = _authModelFactory.PrepareAuthResponseModel(user, token, DateTime.UtcNow.AddHours(24));

            return Ok(model);
        }
    }
}
