namespace InventoryManagementAPI.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> RoleNames { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }
}
