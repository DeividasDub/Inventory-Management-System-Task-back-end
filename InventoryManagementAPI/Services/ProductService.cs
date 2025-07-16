using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.Product;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductResponseFactory _productResponseFactory;

        public ProductService(ApplicationDbContext context, IProductResponseFactory productResponseFactory)
        {
            _context = context;
            _productResponseFactory = productResponseFactory;
        }

        public async Task<ProductResponseDto?> CreateProductAsync(CreateProductRequestDto request)
        {
            if (await _context.Products.AnyAsync(p => p.SKU == request.SKU))
            {
                return null;
            }

            var supplier = await _context.Suppliers.FindAsync(request.SupplierId);
            if (supplier == null)
            {
                return null;
            }

            var product = new Product
            {
                Name = request.Name,
                SKU = request.SKU,
                QuantityInStock = request.QuantityInStock,
                Price = request.Price,
                SupplierId = request.SupplierId,
                CreatedOn = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _productResponseFactory.CreateProductResponse(product, supplier);
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductRequestDto request)
        {
            var product = await _context.Products.Include(p => p.Supplier).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return null;
            }

            if (await _context.Products.AnyAsync(p => p.SKU == request.SKU && p.Id != id))
            {
                return null;
            }

            var supplier = await _context.Suppliers.FindAsync(request.SupplierId);
            if (supplier == null)
            {
                return null;
            }

            product.Name = request.Name;
            product.SKU = request.SKU;
            product.QuantityInStock = request.QuantityInStock;
            product.Price = request.Price;
            product.SupplierId = request.SupplierId;

            await _context.SaveChangesAsync();

            return _productResponseFactory.CreateProductResponse(product, supplier);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            return _productResponseFactory.CreateProductResponse(product);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Supplier)
                .ToListAsync();

            return _productResponseFactory.CreateProductResponses(products);
        }

        public async Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(ProductSearchRequestDto searchRequest)
        {
            var query = _context.Products.Include(p => p.Supplier).AsQueryable();

            if (!string.IsNullOrEmpty(searchRequest.Name))
            {
                query = query.Where(p => p.Name.Contains(searchRequest.Name));
            }

            if (!string.IsNullOrEmpty(searchRequest.SKU))
            {
                query = query.Where(p => p.SKU.Contains(searchRequest.SKU));
            }

            if (searchRequest.SupplierId.HasValue)
            {
                query = query.Where(p => p.SupplierId == searchRequest.SupplierId.Value);
            }

            if (searchRequest.LowStockOnly == true && searchRequest.LowStockThreshold.HasValue)
            {
                query = query.Where(p => p.QuantityInStock <= searchRequest.LowStockThreshold.Value);
            }

            var products = await query.ToListAsync();

            return _productResponseFactory.CreateProductResponses(products);
        }
    }
}
