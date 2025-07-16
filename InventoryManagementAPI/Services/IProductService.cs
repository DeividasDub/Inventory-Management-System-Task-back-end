using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IProductService
    {
        Task<Product?> CreateProductAsync(CreateProductRequestDto request);
        Task<Product?> UpdateProductAsync(int id, UpdateProductRequestDto request);
        Task<bool> DeleteProductAsync(int id);
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> SearchProductsAsync(ProductSearchRequestDto searchRequest);
    }
}
