using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementAPI.DTOs.DatabaseBackup;
using InventoryManagementAPI.Services;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DatabaseBackupController : ControllerBase
    {
        private readonly IDatabaseBackupService _databaseBackupService;

        public DatabaseBackupController(IDatabaseBackupService databaseBackupService)
        {
            _databaseBackupService = databaseBackupService;
        }

        [HttpPost("backup")]
        public async Task<IActionResult> CreateBackup([FromBody] CreateBackupRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var backupPath = await _databaseBackupService.CreateBackupAsync(request.BackupName);
                return Ok(new { message = "Backup created successfully", backupPath = Path.GetFileName(backupPath) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create backup", error = ex.Message });
            }
        }

        [HttpPost("restore")]
        public async Task<IActionResult> RestoreBackup([FromBody] RestoreBackupRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var backupDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "DatabaseBackups");
            var backupFilePath = Path.Combine(backupDirectory, request.BackupFileName);

            if (!System.IO.File.Exists(backupFilePath))
            {
                return NotFound(new { message = "Backup file not found" });
            }

            try
            {
                var success = await _databaseBackupService.RestoreBackupAsync(backupFilePath);
                
                if (success)
                {
                    return Ok(new { message = "Database restored successfully" });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to restore database" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to restore database", error = ex.Message });
            }
        }

        [HttpGet("backups")]
        public async Task<IActionResult> GetAvailableBackups()
        {
            try
            {
                var backups = await _databaseBackupService.GetAvailableBackupsAsync();
                return Ok(new { backups });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve backups", error = ex.Message });
            }
        }
    }
}
