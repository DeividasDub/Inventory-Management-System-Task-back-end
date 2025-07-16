using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs.Role
{
    public class UpdateRoleRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
