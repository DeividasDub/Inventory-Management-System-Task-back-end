using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Services;
using System.Security.Claims;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.CreateProductAsync(request);
            
            if (result == null)
            {
                return BadRequest(new { message = "SKU already exists or supplier not found" });
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.UpdateProductAsync(id, request);
            
            if (result == null)
            {
                return NotFound(new { message = "Product not found, SKU already exists, or supplier not found" });
            }

            return Ok(result);
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
            var result = await _productService.GetProductByIdAsync(id);
            
            if (result == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpPost("search")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> SearchProducts([FromBody] ProductSearchRequestDto searchRequest)
        {
            var products = await _productService.SearchProductsAsync(searchRequest);
            return Ok(products);
        }
    }
}
