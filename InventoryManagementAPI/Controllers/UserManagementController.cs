using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Services;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IUserModelFactory _userModelFactory;

        public UserManagementController(IUserManagementService userManagementService, IUserModelFactory userModelFactory)
        {
            _userManagementService = userManagementService;
            _userModelFactory = userModelFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManagementService.CreateUserAsync(request);
            
            if (user == null)
            {
                return BadRequest(new { message = "User with this email already exists or role not found" });
            }

            var model = _userModelFactory.PrepareUserResponseModel(user);

            return Ok(model);
        }

        [HttpPut("{userId}/role")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManagementService.UpdateUserRoleAsync(userId, request.RoleId);
            
            if (user == null)
            {
                return NotFound(new { message = "User or role not found" });
            }

            var model = _userModelFactory.PrepareUserResponseModel(user);

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            var model = _userModelFactory.PrepareUserListResponseModel(users);
            return Ok(model);
        }

        [HttpDelete("{userId}")]
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
