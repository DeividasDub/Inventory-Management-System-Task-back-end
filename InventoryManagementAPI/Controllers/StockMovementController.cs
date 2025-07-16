using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.StockMovement;
using InventoryManagementAPI.Services;
using System.Security.Claims;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Staff")]
    public class StockMovementController : ControllerBase
    {
        private readonly IStockMovementService _stockMovementService;

        public StockMovementController(IStockMovementService stockMovementService)
        {
            _stockMovementService = stockMovementService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStockMovement([FromBody] CreateStockMovementRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var result = await _stockMovementService.CreateStockMovementAsync(request, userId);
            
            if (result == null)
            {
                return BadRequest(new { message = "Product not found, insufficient stock, or invalid user" });
            }

            return Ok(result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetLastStockMovements(int productId, [FromQuery] int count = 10)
        {
            var stockMovements = await _stockMovementService.GetLastStockMovementsAsync(productId, count);
            return Ok(stockMovements);
        }
    }
}
