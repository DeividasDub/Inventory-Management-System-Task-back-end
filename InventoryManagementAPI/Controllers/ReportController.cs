using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Services;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Staff")]
    public class ReportController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStockMovementService _stockMovementService;

        public ReportController(IProductService productService, IStockMovementService stockMovementService)
        {
            _productService = productService;
            _stockMovementService = stockMovementService;
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 10)
        {
            var searchRequest = new ProductSearchRequestDto
            {
                LowStockOnly = true,
                LowStockThreshold = threshold
            };

            var products = await _productService.SearchProductsAsync(searchRequest);
            return Ok(products);
        }

        [HttpGet("stock-movements/{productId}")]
        public async Task<IActionResult> GetProductStockMovements(int productId, [FromQuery] int count = 10)
        {
            var stockMovements = await _stockMovementService.GetLastStockMovementsAsync(productId, count);
            return Ok(stockMovements);
        }
    }
}
