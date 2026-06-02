using Backend.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

// Takes a full backup of the SQLite database every day at 00:00.
// The same backup logic can also be triggered manually via BackupController.
public class BackupService(IServiceProvider services, ILogger<BackupService> logger) : BackgroundService
{
    // Retention rules: keep backups from the last 7 days, and cap total size at 300 MB.
    private const int RetentionDays = 7;
    private const long MaxTotalBytes = 300L * 1024 * 1024;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Wait until the next midnight
            var now = DateTime.Now;
            var nextMidnight = now.Date.AddDays(1);

            try
            {
                await Task.Delay(nextMidnight - now, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break; // Application is shutting down
            }

            try
            {
                using var scope = services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var path = TakeBackup(db);
                logger.LogInformation("Automatic database backup created: {Path}", path);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Automatic database backup failed.");
            }
        }
    }

    // Creates a backup of the database and returns the path of the new backup file.
    public static string TakeBackup(AppDbContext db)
    {
        var sourcePath = db.Database.GetDbConnection().DataSource;
        if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
            throw new FileNotFoundException("Database file not found.", sourcePath);

        var backupDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
        Directory.CreateDirectory(backupDir);

        var destPath = Path.Combine(backupDir, $"inventory-{DateTime.Now:yyyy-MM-dd_HHmmss}.db");

        // Use SQLite's official backup API for a safe copy (no WAL/lock issues)
        using (var source = new SqliteConnection($"Data Source={sourcePath}"))
        using (var dest = new SqliteConnection($"Data Source={destPath}"))
        {
            source.Open();
            dest.Open();
            source.BackupDatabase(dest);
        }

        CleanupOldBackups(backupDir);
        return destPath;
    }

    // Removes backups older than RetentionDays, then deletes the oldest ones
    // until the total backup size is under MaxTotalBytes.
    private static void CleanupOldBackups(string backupDir)
    {
        var backups = new DirectoryInfo(backupDir)
            .GetFiles("inventory-*.db")
            .OrderBy(f => f.LastWriteTime)
            .ToList();

        // 1) Delete anything older than the retention window
        var cutoff = DateTime.Now.AddDays(-RetentionDays);
        foreach (var file in backups.Where(f => f.LastWriteTime < cutoff).ToList())
        {
            file.Delete();
            backups.Remove(file);
        }

        // 2) If still over the size cap, delete oldest first until under the limit
        //    (always keep at least the most recent backup)
        var totalSize = backups.Sum(f => f.Length);
        while (totalSize > MaxTotalBytes && backups.Count > 1)
        {
            var oldest = backups[0];
            totalSize -= oldest.Length;
            oldest.Delete();
            backups.RemoveAt(0);
        }
    }
}
