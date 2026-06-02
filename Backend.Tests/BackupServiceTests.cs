using Backend.Data;
using Backend.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests;

// TakeBackup needs a real on-disk SQLite file (it checks File.Exists on the
// DataSource), so these tests use a temp file DB instead of the in-memory base.
public class BackupServiceTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;
    private readonly string _backupDir =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");

    public BackupServiceTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"backup-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
    }

    [Fact]
    public void TakeBackup_CreatesNonEmptyBackupFile()
    {
        var path = BackupService.TakeBackup(_db);

        Assert.True(File.Exists(path), "Backup file was not created.");
        Assert.True(new FileInfo(path).Length > 0, "Backup file is empty.");
        Assert.Equal(_backupDir, Path.GetDirectoryName(path));
    }

    [Fact]
    public void TakeBackup_CopiesData()
    {
        // Seed a user into the source DB, then verify it survives the backup copy.
        _db.Users.Add(new Backend.Models.User
        {
            Username = "backup-user",
            Email = "backup@test.local",
            PasswordHash = "hash",
            UserType = "Staff"
        });
        _db.SaveChanges();

        var path = BackupService.TakeBackup(_db);

        using var conn = new SqliteConnection($"Data Source={path}");
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = 'backup-user';";
        var count = Convert.ToInt64(cmd.ExecuteScalar());

        Assert.Equal(1, count);
    }

    [Fact]
    public void TakeBackup_MissingDatabaseFile_Throws()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.db")}")
            .Options;
        using var missingDb = new AppDbContext(options);

        Assert.Throws<FileNotFoundException>(() => BackupService.TakeBackup(missingDb));
    }

    [Fact]
    public void TakeBackup_DeletesBackupsOlderThanRetentionWindow()
    {
        Directory.CreateDirectory(_backupDir);
        var stalePath = Path.Combine(_backupDir, "inventory-2000-01-01_000000.db");
        File.WriteAllText(stalePath, "old backup");
        File.SetLastWriteTime(stalePath, DateTime.Now.AddDays(-10));

        BackupService.TakeBackup(_db);

        Assert.False(File.Exists(stalePath), "Backup older than retention window was not deleted.");
    }

    [Fact]
    public void TakeBackup_KeepsRecentBackups()
    {
        Directory.CreateDirectory(_backupDir);
        var recentPath = Path.Combine(_backupDir, $"inventory-recent-{Guid.NewGuid():N}.db");
        File.WriteAllText(recentPath, "recent backup");
        File.SetLastWriteTime(recentPath, DateTime.Now.AddDays(-1));

        BackupService.TakeBackup(_db);

        Assert.True(File.Exists(recentPath), "A backup within the retention window was deleted.");
        File.Delete(recentPath);
    }

    public void Dispose()
    {
        _db.Dispose();
        SqliteConnection.ClearAllPools();
        if (File.Exists(_dbPath)) File.Delete(_dbPath);
    }
}
