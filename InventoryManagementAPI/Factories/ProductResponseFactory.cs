using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class ProductModelFactory : IProductModelFactory
    {
        public ProductResponseDto PrepareProductResponseModel(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                QuantityInStock = product.QuantityInStock,
                Price = product.Price,
                SupplierId = product.SupplierId,
                SupplierName = product.Supplier?.Name ?? string.Empty,
                CreatedOn = product.CreatedOn
            };
        }

        public ProductResponseDto PrepareProductResponseModel(Product product, Supplier supplier)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                QuantityInStock = product.QuantityInStock,
                Price = product.Price,
                SupplierId = product.SupplierId,
                SupplierName = supplier.Name,
                CreatedOn = product.CreatedOn
            };
        }

        public IEnumerable<ProductResponseDto> PrepareProductListResponseModel(IEnumerable<Product> products)
        {
            return products.Select(PrepareProductResponseModel);
        }
    }
}
