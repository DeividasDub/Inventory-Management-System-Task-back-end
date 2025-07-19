using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.Supplier;
using InventoryManagementAPI.Services;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierModelFactory _supplierModelFactory;

        public SupplierController(
            ISupplierService supplierService, 
            ISupplierModelFactory supplierModelFactory)
        {
            _supplierService = supplierService;
            _supplierModelFactory = supplierModelFactory;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supplier = await _supplierService.CreateSupplierAsync(request);
            
            if (supplier == null)
            {
                return BadRequest(new { message = "Supplier with this email already exists" });
            }

            var model = _supplierModelFactory.PrepareSupplierResponseModel(supplier);

            return Ok(model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supplier = await _supplierService.UpdateSupplierAsync(id, request);
            
            if (supplier == null)
            {
                return NotFound(new { message = "Supplier not found or email already exists" });
            }

            var model = _supplierModelFactory.PrepareSupplierResponseModel(supplier);

            return Ok(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            
            if (supplier == null)
            {
                return NotFound(new { message = "Supplier not found" });
            }

            var model = _supplierModelFactory.PrepareSupplierResponseModel(supplier);

            return Ok(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();

            var model = _supplierModelFactory.PrepareSupplierListResponseModel(suppliers);

            return Ok(model);
        }
    }
}
