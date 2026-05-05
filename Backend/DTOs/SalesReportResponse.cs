namespace Backend.DTOs;

public class SalesReportResponse
{
    public double TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public List<TopSellingItem> TopSellingItems { get; set; } = [];
}

public class TopSellingItem
{
    public string ItemName { get; set; } = "";
    public int TotalQuantitySold { get; set; }
}
