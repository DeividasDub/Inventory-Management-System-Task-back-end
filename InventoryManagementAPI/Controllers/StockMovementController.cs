using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.StockMovement;
using InventoryManagementAPI.Services;
using InventoryManagementAPI.Factories;
using System.Security.Claims;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockMovementController : ControllerBase
    {
        private readonly IStockMovementService _stockMovementService;
        private readonly IStockMovementModelFactory _stockMovementModelFactory;

        public StockMovementController(
            IStockMovementService stockMovementService, 
            IStockMovementModelFactory stockMovementModelFactory)
        {
            _stockMovementService = stockMovementService;
            _stockMovementModelFactory = stockMovementModelFactory;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
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

            var stockMovement = await _stockMovementService.CreateStockMovementAsync(request, userId);
            
            if (stockMovement == null)
            {
                return BadRequest(new { message = "Product not found, insufficient stock, or user not found" });
            }

            var model = _stockMovementModelFactory.PrepareStockMovementResponseModel(stockMovement);

            return Ok(model);
        }

        [HttpGet("product/{productId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetLastStockMovements(int productId, [FromQuery] int count = 10)
        {
            var stockMovements = await _stockMovementService.GetLastStockMovementsAsync(productId, count);

            var model = _stockMovementModelFactory.PrepareStockMovementListResponseModel(stockMovements);

            return Ok(model);
        }
    }
}
