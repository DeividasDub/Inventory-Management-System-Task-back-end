using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IProductResponseFactory
    {
        ProductResponseDto CreateProductResponse(Product product);
        ProductResponseDto CreateProductResponse(Product product, Supplier supplier);
        IEnumerable<ProductResponseDto> CreateProductResponses(IEnumerable<Product> products);
    }
}
