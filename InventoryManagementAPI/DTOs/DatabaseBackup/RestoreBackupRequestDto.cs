using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs.DatabaseBackup
{
    public class RestoreBackupRequestDto
    {
        [Required]
        public string BackupFileName { get; set; } = string.Empty;
    }
}
