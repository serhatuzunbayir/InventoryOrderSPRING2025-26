namespace DesktopApp.Models;

// Stores locally saved desktop settings.
public class DesktopAppOptions
{
    public int LowStockThreshold { get; set; } = 5;
    public int PollingRateSeconds { get; set; } = 15;
}
