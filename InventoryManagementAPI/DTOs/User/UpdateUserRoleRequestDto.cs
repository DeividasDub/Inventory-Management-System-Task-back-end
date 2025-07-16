using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs.User
{
    public class UpdateUserRoleRequestDto
    {
        [Required]
        public int RoleId { get; set; }
    }
}
