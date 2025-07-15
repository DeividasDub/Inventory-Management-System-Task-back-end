using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string SKU { get; set; } = string.Empty;
        
        public int QuantityInStock { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
