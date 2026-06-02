using System.ComponentModel;
using DesktopApp.Models;
using DesktopApp.Services;

namespace DesktopApp;

public partial class MainForm : Form
{
    private readonly ApiClient _apiClient;
    private readonly AppOptionsService _optionsService;
    private readonly NotificationCoordinator _notificationCoordinator;
    private readonly BindingSource _itemBinding = new();
    private readonly BindingSource _orderBinding = new();
    private readonly BindingSource _orderItemsBinding = new();
    private readonly Dictionary<(DataGridView Grid, string Column), ListSortDirection> _sortStates = new();
    // Ignore refresh-driven selection changes.
    private bool _isUpdatingItemSelection;
    // Keep the user's chosen item.
    private int? _selectedItemId;
    // Track whether selection was user-made.
    private bool _hasManualItemSelection;
    public bool LogoutRequested { get; private set; }

    private bool _reportItemsLoaded;

    public MainForm(
        ApiClient apiClient,
        string staffUsername,
        NotificationCoordinator notificationCoordinator,
        AppOptionsService optionsService,
        DesktopAppOptions appOptions)
    {
        // Configure UI bindings and event handlers for the staff console.
        InitializeComponent();

        itemsGrid.AllowUserToAddRows = false;
        ordersGrid.AllowUserToAddRows = false;
        orderItemsGrid.AllowUserToAddRows = false;

        Load += (_, _) => ResizeGrids();
        Resize += (_, _) => ResizeGrids();

        _apiClient = apiClient;
        _optionsService = optionsService;
        _notificationCoordinator = notificationCoordinator;

        lblLoggedIn.Text = $"Staff: {staffUsername}";
        lblBaseUrl.Text = $"API: {_apiClient.BaseUrl}";
        numLowStockThreshold.Value = appOptions.LowStockThreshold;
        numPollingRate.Value = appOptions.PollingRateSeconds;

        itemsGrid.AutoGenerateColumns = true;
        ordersGrid.AutoGenerateColumns = true;
        orderItemsGrid.AutoGenerateColumns = true;
        
        itemsGrid.DataBindingComplete += (_, _) =>
        {
            itemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ApplyMoneyFormatting(itemsGrid);
        };

        ordersGrid.DataBindingComplete += (_, _) =>
        {
            ordersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ApplyMoneyFormatting(ordersGrid);
        };

        orderItemsGrid.DataBindingComplete += (_, _) =>
        {
            orderItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ApplyMoneyFormatting(orderItemsGrid);

            if (orderItemsGrid.Columns["Id"] != null)
                orderItemsGrid.Columns["Id"].Visible = false;
        };

        itemsGrid.DataSource = _itemBinding;
        ordersGrid.DataSource = _orderBinding;
        orderItemsGrid.DataSource = _orderItemsBinding;

        itemsGrid.ColumnAdded += (_, e) => e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
        ordersGrid.ColumnAdded += (_, e) => e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
        orderItemsGrid.ColumnAdded += (_, e) => e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;

        itemsGrid.SelectionChanged += (_, _) => HandleItemSelectionChanged();
        ordersGrid.SelectionChanged += (_, _) => PopulateOrderItems();
        txtItemSearch.KeyDown += async (_, e) => await HandleItemSearchKeyDownAsync(e);

        btnRefreshItems.Click += async (_, _) => await _notificationCoordinator.RefreshItemsAsync();
        btnAddItem.Click += async (_, _) => await AddItemAsync();
        btnUpdateItem.Click += async (_, _) => await UpdateItemAsync();
        btnDeleteItem.Click += async (_, _) => await DeleteItemAsync();
        btnClearItemSearch.Click += async (_, _) => await ClearItemSearchAsync();

        btnRefreshOrders.Click += async (_, _) => await _notificationCoordinator.RefreshOrdersAsync();
        btnUpdateStatus.Click += async (_, _) => await UpdateOrderStatusAsync();
        btnSaveOptions.Click += (_, _) => SaveOptions();
        btnManualBackup.Click += async (_, _) => await TriggerManualBackupAsync();
        btnLogout.Click += (_, _) => HandleLogout();
        tabMain.SelectedIndexChanged += async (_, _) => await HandleTabChangedAsync();

        cmbOrderStatus.Items.AddRange(new object[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });
        cmbOrderStatus.SelectedIndex = 0;

        itemsGrid.ColumnHeaderMouseClick += (_, e) => SortGrid<ItemDto>(itemsGrid, _itemBinding, e.ColumnIndex);
        ordersGrid.ColumnHeaderMouseClick += (_, e) => SortGrid<OrderDto>(ordersGrid, _orderBinding, e.ColumnIndex);
        orderItemsGrid.ColumnHeaderMouseClick += (_, e) =>
            SortGrid<OrderItemDto>(orderItemsGrid, _orderItemsBinding, e.ColumnIndex);

        _notificationCoordinator.ItemsUpdated += BindItems;
        _notificationCoordinator.OrdersUpdated += BindOrders;
        _notificationCoordinator.RefreshFailed += ShowError;

        Load += async (_, _) =>
        {
            // Start notification polling.
            await _notificationCoordinator.StartAsync();
        };

        FormClosed += (_, _) =>
        {
            // Release notif coordinator resources.
            _notificationCoordinator.Stop();
            _notificationCoordinator.Dispose();
        };

        // Enable report actions.
        btnGenerateSales.Click += async (_, _) => await GenerateSalesReportAsync();
        btnGenerateInventory.Click += async (_, _) => await GenerateItemTrendReportAsync();
    }

    // Save current options locally.
    private void SaveOptions()
    {
        var updatedOptions = new DesktopAppOptions
        {
            LowStockThreshold = Convert.ToInt32(numLowStockThreshold.Value),
            PollingRateSeconds = Convert.ToInt32(numPollingRate.Value)
        };

        try
        {
            _optionsService.Save(updatedOptions);
            _notificationCoordinator.ApplyOptions(updatedOptions);

            MessageBox.Show("Options saved locally.", "Options Saved", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowError("Failed to save options.", ex.Message);
        }
    }

    // Call the backup endpoint placeholder.
    private async Task TriggerManualBackupAsync()
    {
        var result = await _apiClient.TriggerManualBackupAsync();
        if (result.Success)
        {
            var fileName = result.Data?.File ?? "Unknown file";
            var message = result.Data?.Message ?? "Backup created.";

            MessageBox.Show($"{message}\n\nFile: {fileName}", "Backup Created", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        MessageBox.Show(
            result.Error,
            "Backup Failed",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    // Apply refreshed items to the grid.
    private void BindItems(IReadOnlyList<ItemDto> items)
    {
        var selectedItemId = _hasManualItemSelection ? _selectedItemId : null;
        var scrollState = CaptureGridScrollState(itemsGrid);

        // Rebind without touching the form.
        _isUpdatingItemSelection = true;
        try
        {
            _itemBinding.DataSource = items.ToList();
            ApplyStoredSort<ItemDto>(itemsGrid, _itemBinding);
            RestoreGridScrollState(itemsGrid, scrollState);

            if (selectedItemId.HasValue && TrySelectItemRow(selectedItemId.Value))
            {
                return;
            }

            _selectedItemId = null;
            _hasManualItemSelection = false;
            ClearItemSelection();
        }
        finally
        {
            _isUpdatingItemSelection = false;
        }
    }

    // Update fields from user selection.
    private void HandleItemSelectionChanged()
    {
        if (_isUpdatingItemSelection)
        {
            return;
        }

        if (!TryGetSelectedItem(out var item))
        {
            _selectedItemId = null;
            _hasManualItemSelection = false;
            ClearItemFields();
            return;
        }

        _selectedItemId = item.Id;
        _hasManualItemSelection = true;
        PopulateItemFields();
    }

    // Fill item fields from selection.
    private void PopulateItemFields()
    {
        if (!TryGetSelectedItem(out var item))
        {
            ClearItemFields();
            return;
        }

        txtItemName.Text = item.Name;
        txtItemCategory.Text = item.Category;
        numItemQuantity.Value = item.Quantity;
        numItemPrice.Value = Convert.ToDecimal(item.Price);
    }

    // Reset the item input fields.
    private void ClearItemFields()
    {
        txtItemName.Text = string.Empty;
        txtItemCategory.Text = string.Empty;
        numItemQuantity.Value = 0;
        numItemPrice.Value = 0;
    }

    // Clear the grid selection state.
    private void ClearItemSelection()
    {
        itemsGrid.ClearSelection();

        if (itemsGrid.Rows.Count > 0)
        {
            itemsGrid.CurrentCell = null;
        }
    }

    // Restore the previous item row.
    private bool TrySelectItemRow(int itemId)
    {
        foreach (DataGridViewRow row in itemsGrid.Rows)
        {
            if (row.DataBoundItem is not ItemDto item || item.Id != itemId)
            {
                continue;
            }

            if (row.Cells.Count > 0)
            {
                itemsGrid.CurrentCell = row.Cells[0];
            }

            row.Selected = true;
            _selectedItemId = itemId;
            _hasManualItemSelection = true;
            return true;
        }

        return false;
    }

    // Read the selected inventory row.
    private bool TryGetSelectedItem(out ItemDto item)
    {
        if (itemsGrid.SelectedRows.Count > 0 &&
            itemsGrid.SelectedRows[0].DataBoundItem is ItemDto selectedItem)
        {
            item = selectedItem;
            return true;
        }

        item = null!;
        return false;
    }

    // Apply search when Enter is pressed.
    private async Task HandleItemSearchKeyDownAsync(KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
        {
            return;
        }

        e.SuppressKeyPress = true;
        await ApplyItemSearchAsync();
    }

    // Refresh the grid with a filter.
    private async Task ApplyItemSearchAsync()
    {
        _notificationCoordinator.SetItemSearchTerm(txtItemSearch.Text);
        await _notificationCoordinator.RefreshItemsAsync();
    }

    // Remove the current item filter.
    private async Task ClearItemSearchAsync()
    {
        txtItemSearch.Text = string.Empty;
        _notificationCoordinator.SetItemSearchTerm(null);
        await _notificationCoordinator.RefreshItemsAsync();
    }

    // Create a new item using the current form inputs.
    private async Task AddItemAsync()
    {
        if (!TryBuildItemRequest(out var request))
        {
            return;
        }

        var result = await _apiClient.CreateItemAsync(request);
        if (!result.Success)
        {
            ShowError("Failed to create item.", result.Error);
            return;
        }

        await _notificationCoordinator.RefreshItemsAsync();
        await EnsureReportItemsLoadedAsync(true);
    }

    // Update the selected item using the current form inputs.
    private async Task UpdateItemAsync()
    {
        if (!TryGetSelectedItem(out var item))
        {
            MessageBox.Show("Select an item to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!TryBuildItemRequest(out var request))
        {
            return;
        }

        var result = await _apiClient.UpdateItemAsync(item.Id, request);
        if (!result.Success)
        {
            ShowError("Failed to update item.", result.Error);
            return;
        }

        await _notificationCoordinator.RefreshItemsAsync();
        await EnsureReportItemsLoadedAsync(true);
    }

    // Delete the selected item after user confirmation.
    private async Task DeleteItemAsync()
    {
        if (!TryGetSelectedItem(out var item))
        {
            MessageBox.Show("Select an item to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show($"Delete '{item.Name}'?", "Confirm Delete", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
        {
            return;
        }

        var result = await _apiClient.DeleteItemAsync(item.Id);
        if (!result.Success)
        {
            ShowError("Failed to delete item.", result.Error);
            return;
        }

        await _notificationCoordinator.RefreshItemsAsync();
        await EnsureReportItemsLoadedAsync(true);
    }

    // Validate and assemble an item request from the form inputs.
    private bool TryBuildItemRequest(out ItemRequest request)
    {
        request = new ItemRequest();
        var name = txtItemName.Text.Trim();
        var category = txtItemCategory.Text.Trim();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category))
        {
            MessageBox.Show("Name and category are required.", "Missing Fields", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        request = new ItemRequest
        {
            Name = name,
            Category = category,
            Quantity = Convert.ToInt32(numItemQuantity.Value),
            Price = Convert.ToDouble(numItemPrice.Value)
        };

        return true;
    }

    // Apply refreshed orders to the grid.
    private void BindOrders(IReadOnlyList<OrderDto> orders)
    {
        var scrollState = CaptureGridScrollState(ordersGrid);
        _orderBinding.DataSource = orders.ToList();
        ApplyStoredSort<OrderDto>(ordersGrid, _orderBinding);
        RestoreGridScrollState(ordersGrid, scrollState);
        PopulateOrderItems();
    }

    // Populate order items grid and status selection from the current order.
    private void PopulateOrderItems()
    {
        var scrollState = CaptureGridScrollState(orderItemsGrid);

        if (ordersGrid.CurrentRow?.DataBoundItem is not OrderDto order)
        {
            _orderItemsBinding.DataSource = new List<OrderItemDto>();
            RestoreGridScrollState(orderItemsGrid, scrollState);
            return;
        }

        _orderItemsBinding.DataSource = order.OrderItems;
        ApplyStoredSort<OrderItemDto>(orderItemsGrid, _orderItemsBinding);
        RestoreGridScrollState(orderItemsGrid, scrollState);
        cmbOrderStatus.SelectedItem = order.Status;
    }

    // Send an updated status for the selected order.
    private async Task UpdateOrderStatusAsync()
    {
        if (ordersGrid.CurrentRow?.DataBoundItem is not OrderDto order)
        {
            MessageBox.Show("Select an order to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (cmbOrderStatus.SelectedItem is not string status)
        {
            MessageBox.Show("Select a status value.", "Missing Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = await _apiClient.UpdateOrderStatusAsync(order.Id, new UpdateOrderStatusRequest
        {
            Status = status
        });

        if (!result.Success)
        {
            ShowError("Failed to update order status.", result.Error);
            return;
        }

        await _notificationCoordinator.RefreshOrdersAsync();
    }

    // Sort the provided grid by the clicked column, toggling direction.
    private void SortGrid<T>(DataGridView grid, BindingSource binding, int columnIndex)
    {
        if (columnIndex < 0 || columnIndex >= grid.Columns.Count)
        {
            return;
        }

        var column = grid.Columns[columnIndex];
        var propertyName = column.DataPropertyName;
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return;
        }

        if (binding.DataSource is not IEnumerable<T> data)
        {
            return;
        }

        var property = typeof(T).GetProperty(propertyName);
        if (property == null)
        {
            return;
        }

        var key = (grid, propertyName);
        var direction = _sortStates.TryGetValue(key, out var current) && current == ListSortDirection.Ascending
            ? ListSortDirection.Descending
            : ListSortDirection.Ascending;

        // Keep one active sort per grid.
        ClearGridSortState(grid);
        _sortStates[key] = direction;

        var sorted = direction == ListSortDirection.Ascending
            ? data.OrderBy(item => property.GetValue(item, null)).ToList()
            : data.OrderByDescending(item => property.GetValue(item, null)).ToList();

        binding.DataSource = sorted;

        var liveColumn = grid.Columns
            .Cast<DataGridViewColumn>()
            .FirstOrDefault(col => col.DataPropertyName == propertyName);
        if (liveColumn != null && liveColumn.DataGridView == grid)
        {
            UpdateSortGlyph(grid, liveColumn, direction);
        }
    }

    // Reapply the saved sort after refresh.
    private void ApplyStoredSort<T>(DataGridView grid, BindingSource binding)
    {
        var sortState = _sortStates.FirstOrDefault(entry => entry.Key.Grid == grid);
        if (sortState.Key.Grid != grid)
        {
            return;
        }

        if (binding.DataSource is not IEnumerable<T> data)
        {
            return;
        }

        var property = typeof(T).GetProperty(sortState.Key.Column);
        if (property == null)
        {
            return;
        }

        var sorted = sortState.Value == ListSortDirection.Ascending
            ? data.OrderBy(item => property.GetValue(item, null)).ToList()
            : data.OrderByDescending(item => property.GetValue(item, null)).ToList();

        binding.DataSource = sorted;

        var liveColumn = grid.Columns
            .Cast<DataGridViewColumn>()
            .FirstOrDefault(col => col.DataPropertyName == sortState.Key.Column);
        if (liveColumn != null && liveColumn.DataGridView == grid)
        {
            UpdateSortGlyph(grid, liveColumn, sortState.Value);
        }
    }

    // Drop stale sort entries for a grid.
    private void ClearGridSortState(DataGridView grid)
    {
        var keysToRemove = _sortStates.Keys
            .Where(key => key.Grid == grid)
            .ToList();

        foreach (var key in keysToRemove)
        {
            _sortStates.Remove(key);
        }
    }

    // Apply the sort glyph to the active column and clear other columns.
    private static void UpdateSortGlyph(DataGridView grid, DataGridViewColumn sortedColumn, ListSortDirection direction)
    {
        foreach (DataGridViewColumn column in grid.Columns)
        {
            if (column.DataGridView == grid)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        if (sortedColumn.DataGridView == grid)
        {
            sortedColumn.HeaderCell.SortGlyphDirection = direction == ListSortDirection.Ascending
                ? SortOrder.Ascending
                : SortOrder.Descending;
        }
    }

    private static void ApplyMoneyFormatting(DataGridView grid)
    {
        var priceColumn = grid.Columns["Price"];
        if (priceColumn == null)
        {
            return;
        }

        priceColumn.HeaderText = "Price ($)";
        priceColumn.DefaultCellStyle.Format = "$#,0.00";
    }

    // Keep the current viewport during refresh.
    private static (int? RowIndex, int HorizontalOffset) CaptureGridScrollState(DataGridView grid)
    {
        int? rowIndex = null;

        if (grid.Rows.Count > 0)
        {
            try
            {
                rowIndex = grid.FirstDisplayedScrollingRowIndex;
            }
            catch (InvalidOperationException)
            {
                rowIndex = null;
            }
        }

        return (rowIndex, grid.HorizontalScrollingOffset);
    }

    // Restore the previous viewport when possible.
    private static void RestoreGridScrollState(DataGridView grid, (int? RowIndex, int HorizontalOffset) scrollState)
    {
        if (grid.Columns.Count > 0)
        {
            grid.HorizontalScrollingOffset = Math.Max(0, scrollState.HorizontalOffset);
        }

        if (!scrollState.RowIndex.HasValue || grid.Rows.Count == 0)
        {
            return;
        }

        var rowIndex = Math.Min(scrollState.RowIndex.Value, grid.Rows.Count - 1);
        if (rowIndex < 0)
        {
            return;
        }

        try
        {
            grid.FirstDisplayedScrollingRowIndex = rowIndex;
        }
        catch (InvalidOperationException)
        {
            // Ignore rows that cannot be displayed yet.
        }
    }

    private static void ShowError(string title, string error)
    {
        MessageBox.Show(error, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    // Close the console and return to login.
    private void HandleLogout()
    {
        var confirm = MessageBox.Show("Log out and return to the login screen?", "Confirm Logout",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
        {
            return;
        }

        LogoutRequested = true;
        Close();
    }

    // Generate and display the sales report.
    private async Task GenerateSalesReportAsync()
    {
        var result = await _apiClient.GetSalesReportAsync();
        if (!result.Success)
        {
            ShowError("Failed to generate sales report.", result.Error);
            return;
        }

        var report = result.Data;
        if (report == null)
        {
            MessageBox.Show("No report data received.", "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        lblTotalRevenue.Text = $"Total Revenue: ${report.TotalRevenue:F2}";
        lblTotalOrders.Text = $"Total Orders: {report.TotalOrders}";

        lstTopItems.Items.Clear();
        foreach (var item in report.TopSellingItems)
        {
            lstTopItems.Items.Add($"{item.ItemName} - {item.TotalQuantitySold} sold");
        }
    }

    // Load report items when the tab opens.
    private async Task HandleTabChangedAsync()
    {
        if (tabMain.SelectedTab == tabReports)
        {
            await EnsureReportItemsLoadedAsync();
        }
    }

    // Refresh the report combo item list.
    private async Task EnsureReportItemsLoadedAsync(bool forceReload = false)
    {
        if (_reportItemsLoaded && !forceReload)
        {
            return;
        }

        var currentItemId = (cmbTrendItems.SelectedItem as ItemDto)?.Id;
        var result = await _apiClient.GetItemsAsync();
        if (!result.Success)
        {
            ShowError("Failed to load report items.", result.Error);
            return;
        }

        var items = (result.Data ?? [])
            .OrderBy(item => item.Name)
            .ToList();

        // Preserve the chosen report item when possible.
        cmbTrendItems.DataSource = items;
        cmbTrendItems.DisplayMember = nameof(ItemDto.Name);
        cmbTrendItems.ValueMember = nameof(ItemDto.Id);
        cmbTrendItems.SelectedIndex = -1;
        cmbTrendItems.Text = string.Empty;

        if (currentItemId.HasValue)
        {
            var restoredIndex = items.FindIndex(item => item.Id == currentItemId.Value);
            if (restoredIndex >= 0)
            {
                cmbTrendItems.SelectedIndex = restoredIndex;
            }
        }

        _reportItemsLoaded = true;
    }

    // Load the selected item's 7-day trend.
    private async Task GenerateItemTrendReportAsync()
    {
        await EnsureReportItemsLoadedAsync();

        var selectedItem = ResolveTrendItemSelection();
        if (selectedItem == null)
        {
            MessageBox.Show("Select an item for the weekly trend.", "No Item Selected", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var result = await _apiClient.GetItemTrendReportAsync(selectedItem.Id);
        if (!result.Success)
        {
            ShowError("Failed to generate weekly sale trend.", result.Error);
            return;
        }

        if (result.Data == null)
        {
            MessageBox.Show("No trend data received.", "Trend Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        BindTrendChart(result.Data);
    }

    // Match the typed item name when needed.
    private ItemDto? ResolveTrendItemSelection()
    {
        if (cmbTrendItems.SelectedItem is ItemDto selectedItem)
        {
            return selectedItem;
        }

        if (cmbTrendItems.DataSource is not List<ItemDto> items)
        {
            return null;
        }

        var typedName = cmbTrendItems.Text.Trim();
        if (string.IsNullOrWhiteSpace(typedName))
        {
            return null;
        }

        var match = items.FirstOrDefault(item =>
            string.Equals(item.Name, typedName, StringComparison.OrdinalIgnoreCase));
        if (match == null)
        {
            return null;
        }

        cmbTrendItems.SelectedItem = match;
        return match;
    }

    // Draw the selected item's weekly sales graph.
    private void BindTrendChart(ItemTrendReportDto report)
    {
        var points = report.Points
            .OrderBy(point => point.Day)
            .ToList();

        var bitmap = new Bitmap(picWeeklyTrend.Width, picWeeklyTrend.Height);
        using var graphics = Graphics.FromImage(bitmap);
        using var axisPen = new Pen(Color.DimGray, 1);
        using var linePen = new Pen(Color.DodgerBlue, 3);
        using var pointBrush = new SolidBrush(Color.DodgerBlue);
        using var labelBrush = new SolidBrush(Color.Black);
        using var gridPen = new Pen(Color.Gainsboro, 1);

        graphics.Clear(Color.White);

        const int leftMargin = 72;
        const int rightMargin = 36;
        const int topMargin = 28;
        const int bottomMargin = 34;

        var plotWidth = bitmap.Width - leftMargin - rightMargin;
        var plotHeight = bitmap.Height - topMargin - bottomMargin;
        var maxQuantity = Math.Max(1, points.Max(point => point.QuantitySold));
        var plotBottom = topMargin + plotHeight;

        // Draw the base axes
        graphics.DrawLine(axisPen, leftMargin, topMargin, leftMargin, plotBottom);
        graphics.DrawLine(axisPen, leftMargin, plotBottom, leftMargin + plotWidth, plotBottom);

        for (var step = 0; step <= 4; step++)
        {
            var y = topMargin + (plotHeight * step / 4f);
            var value = maxQuantity - (maxQuantity * step / 4f);

            graphics.DrawLine(gridPen, leftMargin, y, leftMargin + plotWidth, y);
            graphics.DrawString(Math.Round(value).ToString("0"), Font, labelBrush, 6, y - 8);
        }

        if (points.Count > 0)
        {
            var plottedPoints = new PointF[points.Count];

            for (var index = 0; index < points.Count; index++)
            {
                var point = points[index];
                var x = points.Count == 1
                    ? leftMargin + (plotWidth / 2f)
                    : leftMargin + (plotWidth * index / (points.Count - 1f));
                var yRatio = point.QuantitySold / (float)maxQuantity;
                var y = plotBottom - (plotHeight * yRatio);

                // Save screen points
                plottedPoints[index] = new PointF(x, y);
                graphics.FillEllipse(pointBrush, x - 4, y - 4, 8, 8);
                graphics.DrawString(point.Day.ToLocalTime().ToString("MMM dd"), Font, labelBrush, x - 18, plotBottom + 6);
                graphics.DrawString(point.QuantitySold.ToString(), Font, labelBrush, x - 10, y - 22);
            }

            if (plottedPoints.Length > 1)
            {
                // Connect the daily points
                graphics.DrawLines(linePen, plottedPoints);
            }
        }

        // Replace graph image
        picWeeklyTrend.Image?.Dispose();
        picWeeklyTrend.Image = bitmap;
        lblTrendChartTitle.Text = $"{report.ItemName} - Last 7 Days";
    }

    private void ResizeGrids()
    {
        int rightLimit = btnRefreshOrders.Left - 12;

        ordersGrid.Width = rightLimit - ordersGrid.Left;
        orderItemsGrid.Width = rightLimit - orderItemsGrid.Left;
    }
}
