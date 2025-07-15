using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs
{
    public class UpdateUserRoleRequestDto
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
