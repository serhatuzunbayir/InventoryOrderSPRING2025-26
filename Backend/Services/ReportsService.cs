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

    public async Task<ItemTrendReportResponse?> GetItemTrendReportAsync(int itemId)
    {
        var itemName = await db.Items
            .Where(i => i.Id == itemId)
            .Select(i => i.Name)
            .FirstOrDefaultAsync();

        if (itemName == null)
        {
            return null;
        }

        var startDate = DateTime.UtcNow.Date.AddDays(-6);
        var endDate = startDate.AddDays(7);

        var sales = await db.OrderItems
            .Where(oi => oi.ItemId == itemId &&
                         oi.Order.Status == "Delivered" &&
                         oi.Order.OrderedDate >= startDate &&
                         oi.Order.OrderedDate < endDate)
            .Select(oi => new
            {
                oi.Quantity,
                oi.Order.OrderedDate
            })
            .ToListAsync();

        // Fill in zero-sale days for the chart.
        var points = Enumerable.Range(0, 7)
            .Select(offset =>
            {
                var day = startDate.AddDays(offset);
                return new ItemTrendPoint
                {
                    Day = day,
                    QuantitySold = sales
                        .Where(sale => sale.OrderedDate.Date == day)
                        .Sum(sale => sale.Quantity)
                };
            })
            .ToList();

        return new ItemTrendReportResponse
        {
            ItemId = itemId,
            ItemName = itemName,
            Points = points
        };
    }
}
