using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Services;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Staff")]
    public class ReportController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStockMovementService _stockMovementService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IStockMovementModelFactory _stockMovementModelFactory;

        public ReportController(
            IProductService productService, 
            IStockMovementService stockMovementService, 
            IProductModelFactory productModelFactory, 
            IStockMovementModelFactory stockMovementModelFactory)
        {
            _productService = productService;
            _stockMovementService = stockMovementService;
            _productModelFactory = productModelFactory;
            _stockMovementModelFactory = stockMovementModelFactory;
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

            var model = _productModelFactory.PrepareProductListResponseModel(products);

            return Ok(model);
        }

        [HttpGet("stock-movements/{productId}")]
        public async Task<IActionResult> GetProductStockMovements(int productId, [FromQuery] int count = 10)
        {
            var stockMovements = await _stockMovementService.GetLastStockMovementsAsync(productId, count);

            var model = _stockMovementModelFactory.PrepareStockMovementListResponseModel(stockMovements);

            return Ok(model);
        }
    }
}
