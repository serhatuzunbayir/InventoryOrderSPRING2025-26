namespace Backend.DTOs;

public class ItemTrendReportResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = "";
    public List<ItemTrendPoint> Points { get; set; } = [];
}

public class ItemTrendPoint
{
    public DateTime Day { get; set; }
    public int QuantitySold { get; set; }
}
