using InventoryManagementAPI.DTOs;

namespace InventoryManagementAPI.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto?> CreateProductAsync(CreateProductRequestDto request);
        Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductRequestDto request);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(ProductSearchRequestDto searchRequest);
    }
}
