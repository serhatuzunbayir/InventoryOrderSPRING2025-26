using DesktopApp.Models;

namespace DesktopApp.Services;

public class NotificationCoordinator : IDisposable
{
    private readonly ApiClient _apiClient;
    private readonly NotificationService _notificationService;
    // Background polling timer.
    private readonly System.Windows.Forms.Timer _notificationTimer = new();
    // Keep the active item filter.
    private string? _itemSearchTerm;
    // Prevent overlapping refresh work.
    private bool _isRefreshInProgress;
    private bool _isInitialized;

    public event Action<IReadOnlyList<OrderDto>>? OrdersUpdated;
    public event Action<IReadOnlyList<ItemDto>>? ItemsUpdated;
    public event Action<string, string>? RefreshFailed;

    public NotificationCoordinator(ApiClient apiClient, NotificationService notificationService, DesktopAppOptions options)
    {
        _apiClient = apiClient;
        _notificationService = notificationService;

        // Apply saved startup settings.
        ApplyOptions(options);
        _notificationTimer.Tick += async (_, _) => await HandleNotificationTimerTickAsync();
    }

    // Apply updated runtime settings.
    public void ApplyOptions(DesktopAppOptions options)
    {
        _notificationService.UpdateLowStockThreshold(options.LowStockThreshold);
        _notificationTimer.Interval = Math.Max(5, options.PollingRateSeconds) * 1000;
    }

    // Store the current item filter.
    public void SetItemSearchTerm(string? searchTerm)
    {
        _itemSearchTerm = string.IsNullOrWhiteSpace(searchTerm)
            ? null
            : searchTerm.Trim();
    }

    public async Task StartAsync()
    {
        // Seed notification state on startup.
        var initialized = await EnsureInitializedAsync();
        if (initialized)
        {
            _notificationTimer.Start();
        }
    }

    public void Stop()
    {
        // Stop polling when form closes.
        _notificationTimer.Stop();
    }

    public async Task RefreshOrdersAsync()
    {
        // Recover initialization from refresh.
        if (!await EnsureInitializedAsync())
        {
            return;
        }

        var orders = await FetchOrdersAsync();
        if (orders == null)
        {
            return;
        }

        OrdersUpdated?.Invoke(orders);
        _notificationService.CheckForNewOrders(orders);
    }

    public async Task RefreshItemsAsync()
    {
        // Recover initialization from refresh.
        if (!await EnsureInitializedAsync())
        {
            return;
        }

        var items = await FetchDisplayItemsAsync();
        if (items == null)
        {
            return;
        }

        ItemsUpdated?.Invoke(items);

        var itemsForNotifications = string.IsNullOrWhiteSpace(_itemSearchTerm)
            ? items
            : await FetchNotificationItemsAsync();

        if (itemsForNotifications != null)
        {
            _notificationService.CheckForLowStock(itemsForNotifications);
        }
    }

    public void Dispose()
    {
        _notificationTimer.Dispose();
    }

    private async Task<bool> EnsureInitializedAsync()
    {
        if (_isInitialized)
        {
            return true;
        }

        var items = await FetchNotificationItemsAsync();
        var orders = await FetchOrdersAsync();

        if (items == null || orders == null)
        {
            return false;
        }

        var displayItems = await FetchDisplayItemsAsync(items);
        if (displayItems == null)
        {
            return false;
        }

        ItemsUpdated?.Invoke(displayItems);
        OrdersUpdated?.Invoke(orders);
        _notificationService.Initialize(orders, items);
        _isInitialized = true;
        return true;
    }

    private async Task<List<OrderDto>?> FetchOrdersAsync()
    {
        var result = await _apiClient.GetOrdersAsync();
        if (!result.Success)
        {
            RefreshFailed?.Invoke("Failed to load orders.", result.Error);
            return null;
        }

        return result.Data ?? [];
    }

    private async Task<List<ItemDto>?> FetchDisplayItemsAsync(List<ItemDto>? allItems = null)
    {
        if (string.IsNullOrWhiteSpace(_itemSearchTerm))
        {
            return allItems ?? await FetchNotificationItemsAsync();
        }

        var result = await _apiClient.GetItemsAsync(_itemSearchTerm);
        if (!result.Success)
        {
            RefreshFailed?.Invoke("Failed to load items.", result.Error);
            return null;
        }

        return result.Data ?? [];
    }

    // Load the full item list.
    private async Task<List<ItemDto>?> FetchNotificationItemsAsync(List<ItemDto>? allItems = null)
    {
        if (allItems != null)
        {
            return allItems;
        }

        var result = await _apiClient.GetItemsAsync();
        if (!result.Success)
        {
            RefreshFailed?.Invoke("Failed to load items.", result.Error);
            return null;
        }

        return result.Data ?? [];
    }

    private async Task HandleNotificationTimerTickAsync()
    {
        // Skip reentrant timer ticks.
        if (_isRefreshInProgress || !_isInitialized)
        {
            return;
        }

        _isRefreshInProgress = true;

        try
        {
            var orders = await FetchOrdersAsync();
            if (orders != null)
            {
                OrdersUpdated?.Invoke(orders);
                _notificationService.CheckForNewOrders(orders);
            }

            var items = await FetchNotificationItemsAsync();
            if (items != null)
            {
                var displayItems = await FetchDisplayItemsAsync(items);
                if (displayItems == null)
                {
                    return;
                }

                ItemsUpdated?.Invoke(displayItems);
                _notificationService.CheckForLowStock(items);
            }
        }
        finally
        {
            _isRefreshInProgress = false;
        }
    }
}
