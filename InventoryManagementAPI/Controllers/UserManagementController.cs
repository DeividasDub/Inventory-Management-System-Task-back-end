using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Services;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpPost("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManagementService.CreateUserAsync(request);
            
            if (result == null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            return Ok(result);
        }

        [HttpPut("{userId}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManagementService.UpdateUserRoleAsync(userId, request.RoleId);
            
            if (result == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(result);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userManagementService.DeleteUserAsync(userId);
            
            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
