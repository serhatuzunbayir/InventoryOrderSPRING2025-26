namespace Backend.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    // Price captured at order time — unaffected by future price changes
    public double Price { get; set; }

    public Order Order { get; set; } = null!;
    public Item Item { get; set; } = null!;
}
