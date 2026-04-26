using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class OrderService(AppDbContext db)
{
    private static readonly string[] ValidStatuses =
        ["Pending", "Processing", "Shipped", "Delivered", "Cancelled"];

    public async Task<List<Order>> GetAllAsync(int userId, string role)
    {
        var query = db.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Item)
            .AsQueryable();

        if (role == "Customer")
            query = query.Where(o => o.UserId == userId);

        return await query.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await db.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Item)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<ServiceResult<Order>> CreateAsync(int userId, CreateOrderRequest req)
    {
        var orderItems = new List<OrderItem>();

        foreach (var reqItem in req.Items)
        {
            var item = await db.Items.FindAsync(reqItem.ItemId);
            if (item == null)
                return ServiceResult<Order>.Fail($"Item {reqItem.ItemId} not found.");

            if (item.Quantity < reqItem.Quantity)
                return ServiceResult<Order>.Fail($"Insufficient stock for item '{item.Name}'.");

            item.Quantity -= reqItem.Quantity;

            orderItems.Add(new OrderItem
            {
                ItemId = item.Id,
                Quantity = reqItem.Quantity,
                Price = item.Price // price snapshot at order time
            });
        }

        var order = new Order
        {
            UserId = userId,
            OrderItems = orderItems
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();
        return ServiceResult<Order>.Ok(order);
    }

    public async Task<ServiceResult<Order>> UpdateStatusAsync(int id, UpdateOrderStatusRequest req)
    {
        if (!ValidStatuses.Contains(req.Status))
            return ServiceResult<Order>.Fail("Invalid status value.");

        var order = await db.Orders.FindAsync(id);
        if (order == null)
            return ServiceResult<Order>.Fail("Order not found.");

        order.Status = req.Status;
        await db.SaveChangesAsync();
        return ServiceResult<Order>.Ok(order);
    }
}
