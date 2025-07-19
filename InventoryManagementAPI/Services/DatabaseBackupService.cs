using Microsoft.Data.SqlClient;
using InventoryManagementAPI.Data;

namespace InventoryManagementAPI.Services
{
    public class DatabaseBackupService : IDatabaseBackupService
    {
        private readonly IConfiguration _configuration;
        private readonly string _backupDirectory;

        public DatabaseBackupService(IConfiguration configuration)
        {
            _configuration = configuration;
            _backupDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "DatabaseBackups");
            
            if (!Directory.Exists(_backupDirectory))
            {
                Directory.CreateDirectory(_backupDirectory);
            }
        }

        public async Task<string> CreateBackupAsync(string backupName)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var databaseName = GetDatabaseNameFromConnectionString(connectionString);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFileName = $"{backupName}_{timestamp}.bak";
            var backupFilePath = Path.Combine(_backupDirectory, backupFileName);

            var backupQuery = $@"
                BACKUP DATABASE [{databaseName}] 
                TO DISK = @BackupPath 
                WITH FORMAT, INIT, NAME = @BackupName, SKIP, NOREWIND, NOUNLOAD, STATS = 10";

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            
            using var command = new SqlCommand(backupQuery, connection);
            command.Parameters.AddWithValue("@BackupPath", backupFilePath);
            command.Parameters.AddWithValue("@BackupName", $"{backupName} - Full Database Backup");
            
            await command.ExecuteNonQueryAsync();
            
            return backupFilePath;
        }

        public async Task<bool> RestoreBackupAsync(string backupFilePath)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                var databaseName = GetDatabaseNameFromConnectionString(connectionString);
                var masterConnectionString = connectionString.Replace($"Initial Catalog={databaseName}", "Initial Catalog=master");

                using var connection = new SqlConnection(masterConnectionString);
                await connection.OpenAsync();

                var setOfflineQuery = $"ALTER DATABASE [{databaseName}] SET OFFLINE WITH ROLLBACK IMMEDIATE";
                using var offlineCommand = new SqlCommand(setOfflineQuery, connection);
                await offlineCommand.ExecuteNonQueryAsync();

                var restoreQuery = $@"
                    RESTORE DATABASE [{databaseName}] 
                    FROM DISK = @BackupPath 
                    WITH REPLACE, STATS = 10";

                using var restoreCommand = new SqlCommand(restoreQuery, connection);
                restoreCommand.Parameters.AddWithValue("@BackupPath", backupFilePath);
                await restoreCommand.ExecuteNonQueryAsync();

                var setOnlineQuery = $"ALTER DATABASE [{databaseName}] SET ONLINE";
                using var onlineCommand = new SqlCommand(setOnlineQuery, connection);
                await onlineCommand.ExecuteNonQueryAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<string>> GetAvailableBackupsAsync()
        {
            return await Task.FromResult(
                Directory.GetFiles(_backupDirectory, "*.bak")
                    .Select(Path.GetFileName)
                    .OrderByDescending(f => f)
                    .ToList()
            );
        }

        private string GetDatabaseNameFromConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            return builder.InitialCatalog;
        }
    }
}
