using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.Role;
using InventoryManagementAPI.Services;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleManagementController : ControllerBase
    {
        private readonly IRoleManagementService _roleManagementService;

        public RoleManagementController(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleManagementService.CreateRoleAsync(request);
            
            if (result == null)
            {
                return BadRequest(new { message = "Role name already exists" });
            }

            return Ok(result);
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] UpdateRoleRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleManagementService.UpdateRoleAsync(roleId, request);
            
            if (result == null)
            {
                return BadRequest(new { message = "Role not found or name already exists" });
            }

            return Ok(result);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var result = await _roleManagementService.DeleteRoleAsync(roleId);
            
            if (!result)
            {
                return BadRequest(new { message = "Role not found or has associated users" });
            }

            return Ok(new { message = "Role deleted successfully" });
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManagementService.GetAllRolesAsync();
            return Ok(roles);
        }
    }
}
