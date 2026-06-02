using DesktopApp.Models;

namespace DesktopApp.Services;

// Explicit delegate for new orders.
public delegate void OrderPlacedNotificationHandler(OrderDto order);
// Explicit delegate for low stock.
public delegate void LowStockDetectedHandler(ItemDto item, int threshold);

public class NotificationService
{
    private const int LowStockThreshold = 5;

    private readonly HashSet<int> _knownOrderIds = [];
    private readonly HashSet<int> _alertedLowStockItemIds = [];

    private bool _ordersInitialized;
    private bool _itemsInitialized;

    public event OrderPlacedNotificationHandler? OrderPlaced;
    public event LowStockDetectedHandler? LowStockDetected;

    public NotificationService()
    {
        // Register default popup handlers.
        OrderPlaced += ShowOrderPlacedNotification;
        LowStockDetected += ShowLowStockNotification;
    }

    public void Initialize(IEnumerable<OrderDto> orders, IEnumerable<ItemDto> items)
    {
        // Seed state without notifications.
        var orderList = orders.ToList();
        var itemList = items.ToList();

        _knownOrderIds.Clear();
        _alertedLowStockItemIds.Clear();

        foreach (var orderId in orderList.Select(order => order.Id))
        {
            _knownOrderIds.Add(orderId);
        }

        foreach (var itemId in itemList
                     .Where(item => item.Quantity <= LowStockThreshold)
                     .Select(item => item.Id))
        {
            _alertedLowStockItemIds.Add(itemId);
        }

        _ordersInitialized = true;
        _itemsInitialized = true;
    }

    public void CheckForNewOrders(IEnumerable<OrderDto> orders)
    {
        // Notify only unseen orders.
        var orderList = orders.ToList();
        if (!_ordersInitialized)
        {
            Initialize(orderList, []);
            return;
        }

        foreach (var order in orderList
                     .Where(order => !_knownOrderIds.Contains(order.Id))
                     .OrderBy(order => order.OrderedDate)
                     .ThenBy(order => order.Id))
        {
            OrderPlaced?.Invoke(order);
        }

        _knownOrderIds.Clear();
        foreach (var orderId in orderList.Select(order => order.Id))
        {
            _knownOrderIds.Add(orderId);
        }
    }

    public void CheckForLowStock(IEnumerable<ItemDto> items)
    {
        // Notify on low-stock transitions.
        var itemList = items.ToList();
        if (!_itemsInitialized)
        {
            Initialize([], itemList);
            return;
        }

        foreach (var item in itemList.Where(item => item.Quantity <= LowStockThreshold))
        {
            if (_alertedLowStockItemIds.Add(item.Id))
            {
                LowStockDetected?.Invoke(item, LowStockThreshold);
            }
        }

        foreach (var itemId in itemList
                     .Where(item => item.Quantity > LowStockThreshold)
                     .Select(item => item.Id))
        {
            _alertedLowStockItemIds.Remove(itemId);
        }
    }

    private static void ShowOrderPlacedNotification(OrderDto order)
    {
        //Display Messagebox
        MessageBox.Show(
            $"New order received.\n\nOrder ID: {order.Id}\nCustomer ID: {order.UserId}\nOrdered: {order.OrderedDate:g}",
            "New Order",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private static void ShowLowStockNotification(ItemDto item, int threshold)
    {
        //Display Messagebox
        MessageBox.Show(
            $"Item '{item.Name}' is low on stock.\n\nCurrent quantity: {item.Quantity}\nThreshold: {threshold}",
            "Low Stock Alert",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }
}
