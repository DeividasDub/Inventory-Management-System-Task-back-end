using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Services;
using InventoryManagementAPI.Factories;
using System.Security.Claims;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductModelFactory _productModelFactory;

        public ProductController(
            IProductService productService, 
            IProductModelFactory productModelFactory)
        {
            _productService = productService;
            _productModelFactory = productModelFactory;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.CreateProductAsync(request);
            
            if (product == null)
            {
                return BadRequest(new { message = "SKU already exists or supplier not found" });
            }

            var model = _productModelFactory.PrepareProductResponseModel(product);

            return Ok(model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.UpdateProductAsync(id, request);
            
            if (product == null)
            {
                return NotFound(new { message = "Product not found, SKU already exists, or supplier not found" });
            }

            var model = _productModelFactory.PrepareProductResponseModel(product);

            return Ok(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            
            if (!result)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            var model = _productModelFactory.PrepareProductResponseModel(product);

            return Ok(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            var model = _productModelFactory.PrepareProductListResponseModel(products);
            return Ok(model);
        }

        [HttpPost("search")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> SearchProducts([FromBody] ProductSearchRequestDto searchRequest)
        {
            var products = await _productService.SearchProductsAsync(searchRequest);
            var model = _productModelFactory.PrepareProductListResponseModel(products);
            return Ok(model);
        }
    }
}
