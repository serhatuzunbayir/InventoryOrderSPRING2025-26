namespace Backend.DTOs;

public class ItemRequest
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Category { get; set; } = string.Empty;
}
