using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs;
using InventoryManagementAPI.Services;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _supplierService.CreateSupplierAsync(request);
            
            if (result == null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _supplierService.UpdateSupplierAsync(id, request);
            
            if (result == null)
            {
                return NotFound(new { message = "Supplier not found or email already exists" });
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            
            if (!result)
            {
                return BadRequest(new { message = "Supplier not found or has associated products" });
            }

            return Ok(new { message = "Supplier deleted successfully" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var result = await _supplierService.GetSupplierByIdAsync(id);
            
            if (result == null)
            {
                return NotFound(new { message = "Supplier not found" });
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }
    }
}
