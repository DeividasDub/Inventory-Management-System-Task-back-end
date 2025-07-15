using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs
{
    public class UpdateRoleRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
