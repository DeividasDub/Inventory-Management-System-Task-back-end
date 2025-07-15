using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs
{
    public class UpdateUserRoleRequestDto
    {
        [Required]
        public int RoleId { get; set; }
    }
}
