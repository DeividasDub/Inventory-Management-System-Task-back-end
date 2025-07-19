using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.Role;
using InventoryManagementAPI.Services;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleManagementController : ControllerBase
    {
        private readonly IRoleManagementService _roleManagementService;
        private readonly IRoleModelFactory _roleModelFactory;

        public RoleManagementController(
            IRoleManagementService roleManagementService, 
            IRoleModelFactory roleModelFactory)
        {
            _roleManagementService = roleManagementService;
            _roleModelFactory = roleModelFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _roleManagementService.CreateRoleAsync(request);
            
            if (role == null)
            {
                return BadRequest(new { message = "Role with this name already exists" });
            }

            var model = _roleModelFactory.PrepareRoleResponseModel(role);

            return Ok(model);
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] UpdateRoleRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _roleManagementService.UpdateRoleAsync(roleId, request);
            
            if (role == null)
            {
                return NotFound(new { message = "Role not found or name already exists" });
            }

            var model = _roleModelFactory.PrepareRoleResponseModel(role);

            return Ok(model);
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

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManagementService.GetAllRolesAsync();

            var model = _roleModelFactory.PrepareRoleListResponseModel(roles);

            return Ok(model);
        }
    }
}
