using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs.DatabaseBackup
{
    public class CreateBackupRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string BackupName { get; set; } = string.Empty;
    }
}
