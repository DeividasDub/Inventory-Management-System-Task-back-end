using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs
{
    public class CreateRoleRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
