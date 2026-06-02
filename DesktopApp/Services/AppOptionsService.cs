using System.Text.Json;
using DesktopApp.Models;

namespace DesktopApp.Services;

// Loads and saves local app settings.
public class AppOptionsService
{
    private const string ConfigDirectoryName = "InventoryStaffConsole";
    private const string ConfigFileName = "options.json";

    public string ConfigPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            ConfigDirectoryName,
            ConfigFileName);

    // Read settings from disk.
    public DesktopAppOptions Load()
    {
        if (!File.Exists(ConfigPath))
        {
            return new DesktopAppOptions();
        }

        try
        {
            var json = File.ReadAllText(ConfigPath);
            var options = JsonSerializer.Deserialize<DesktopAppOptions>(json);
            return Normalize(options ?? new DesktopAppOptions());
        }
        catch
        {
            return new DesktopAppOptions();
        }
    }

    // Write settings to disk.
    public void Save(DesktopAppOptions options)
    {
        var normalized = Normalize(options);
        var directory = Path.GetDirectoryName(ConfigPath)!;
        Directory.CreateDirectory(directory);

        var json = JsonSerializer.Serialize(normalized, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(ConfigPath, json);
    }

    // Clamp settings to safe values.
    private static DesktopAppOptions Normalize(DesktopAppOptions options)
    {
        options.LowStockThreshold = Math.Clamp(options.LowStockThreshold, 1, 1000000);
        options.PollingRateSeconds = Math.Clamp(options.PollingRateSeconds, 5, 3600);
        return options;
    }
}
