namespace InventoryManagementAPI.Services
{
    public interface IDatabaseBackupService
    {
        Task<string> CreateBackupAsync(string backupName);
        Task<bool> RestoreBackupAsync(string backupFilePath);
        Task<List<string>> GetAvailableBackupsAsync();
    }
}
