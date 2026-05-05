using Backend.Data;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ReportsService(AppDbContext db)
{
    public async Task<SalesReportResponse> GetSalesReportAsync()
    {
        // Get all non-cancelled orders with their items
        var orders = await db.Orders
            .Where(o => o.Status == "Delivered")
            .Include(o => o.OrderItems)
            .ToListAsync();

        var totalOrders = orders.Count;
        var totalRevenue = orders
            .SelectMany(o => o.OrderItems)
            .Sum(oi => oi.Quantity * oi.Price);

        // Top selling items: group by item name, sum quantities
        var topSellingItems = await db.OrderItems
            .Where(oi => db.Orders.Any(o => o.Id == oi.OrderId && o.Status == "Delivered"))
            .GroupBy(oi => oi.Item.Name)
            .Select(g => new TopSellingItem
            {
                ItemName = g.Key,
                TotalQuantitySold = g.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(tsi => tsi.TotalQuantitySold)
            .Take(5)
            .ToListAsync();

        return new SalesReportResponse
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            TopSellingItems = topSellingItems
        };
    }
}
