using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs.Product
{
    public class ProductSearchRequestDto
    {
        public string? Name { get; set; }
        public string? SKU { get; set; }
        public int? SupplierId { get; set; }
        public int? LowStockThreshold { get; set; }
        public bool? LowStockOnly { get; set; }
    }
}
