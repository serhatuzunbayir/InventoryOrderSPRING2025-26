namespace Backend.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Category { get; set; } = string.Empty;

    // Navigation property
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
