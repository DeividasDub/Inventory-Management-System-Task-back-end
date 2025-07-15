using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string ContactName { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
