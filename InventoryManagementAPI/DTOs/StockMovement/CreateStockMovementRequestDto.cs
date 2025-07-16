using System.ComponentModel.DataAnnotations;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.DTOs.StockMovement
{
    public class CreateStockMovementRequestDto
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public StockMovementType Type { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }
}
