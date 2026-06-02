namespace DesktopApp.Models;

public class ItemTrendReportDto
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public List<ItemTrendPointDto> Points { get; set; } = [];
}

public class ItemTrendPointDto
{
    public DateTime Day { get; set; }
    public int QuantitySold { get; set; }
}
