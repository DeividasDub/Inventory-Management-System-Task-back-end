using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StockMovementType
    {
        IN,
        OUT
    }

    public class StockMovement
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        [Required]
        public StockMovementType Type { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;
        
        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
    }
}
