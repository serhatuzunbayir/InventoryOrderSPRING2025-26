namespace Backend.DTOs;

public class CreateOrderRequest
{
    // An order can contain one or more items
    public List<OrderItemRequest> Items { get; set; } = [];
}

public class OrderItemRequest
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateOrderStatusRequest
{
    public string Status { get; set; } = string.Empty;
}
