namespace DesktopApp.Models;

public class SalesReportDto
{
    public double TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public List<TopSellingItemDto> TopSellingItems { get; set; } = [];
}

public class TopSellingItemDto
{
    public string ItemName { get; set; } = "";
    public int TotalQuantitySold { get; set; }
}
