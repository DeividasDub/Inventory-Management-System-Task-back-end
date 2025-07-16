using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class ProductResponseFactory : IProductResponseFactory
    {
        public ProductResponseDto CreateProductResponse(Product product)
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

        public ProductResponseDto CreateProductResponse(Product product, Supplier supplier)
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

        public IEnumerable<ProductResponseDto> CreateProductResponses(IEnumerable<Product> products)
        {
            return products.Select(CreateProductResponse);
        }
    }
}
