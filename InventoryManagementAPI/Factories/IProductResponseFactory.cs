using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IProductModelFactory
    {
        ProductResponseDto PrepareProductResponseModel(Product product);
        ProductResponseDto PrepareProductResponseModel(Product product, Supplier supplier);
        IEnumerable<ProductResponseDto> PrepareProductListResponseModel(IEnumerable<Product> products);
    }
}
