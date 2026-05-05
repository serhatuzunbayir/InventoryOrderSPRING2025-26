using System.ComponentModel;
using DesktopApp.Models;
using DesktopApp.Services;

namespace DesktopApp;

public partial class MainForm : Form
{
    private readonly ApiClient _apiClient;
    private readonly BindingSource _itemBinding = new();
    private readonly BindingSource _orderBinding = new();
    private readonly BindingSource _orderItemsBinding = new();
    private readonly Dictionary<(DataGridView Grid, string Column), ListSortDirection> _sortStates = new();

    // Reports controls
    private Label lblTotalRevenue;
    private Label lblTotalOrders;
    private ListBox lstTopItems;

    public MainForm(ApiClient apiClient, string staffUsername)
    {
        // Configure UI bindings and event handlers for the staff console.
        InitializeComponent();
        
        itemsGrid.AllowUserToAddRows = false;
        ordersGrid.AllowUserToAddRows = false;
        orderItemsGrid.AllowUserToAddRows = false;
        
        this.Load += (_, _) => ResizeGrids();
        this.Resize += (_, _) => ResizeGrids();
        
        _apiClient = apiClient;

        lblLoggedIn.Text = $"Staff: {staffUsername}";
        lblBaseUrl.Text = $"API: {_apiClient.BaseUrl}";

        itemsGrid.AutoGenerateColumns = true;
        ordersGrid.AutoGenerateColumns = true;
        orderItemsGrid.AutoGenerateColumns = true;
        
        ordersGrid.DataBindingComplete += (_, _) =>
        {
            ordersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        };

        orderItemsGrid.DataBindingComplete += (_, _) =>
        {
            orderItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            if (orderItemsGrid.Columns["Id"] != null)
                orderItemsGrid.Columns["Id"].Visible = false;
        };

        itemsGrid.DataSource = _itemBinding;
        ordersGrid.DataSource = _orderBinding;
        orderItemsGrid.DataSource = _orderItemsBinding;

        itemsGrid.ColumnAdded += (_, e) => e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
        ordersGrid.ColumnAdded += (_, e) => e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
        orderItemsGrid.ColumnAdded += (_, e) => e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;

        itemsGrid.SelectionChanged += (_, _) => PopulateItemFields();
        ordersGrid.SelectionChanged += (_, _) => PopulateOrderItems();

        btnRefreshItems.Click += async (_, _) => await LoadItemsAsync();
        btnAddItem.Click += async (_, _) => await AddItemAsync();
        btnUpdateItem.Click += async (_, _) => await UpdateItemAsync();
        btnDeleteItem.Click += async (_, _) => await DeleteItemAsync();

        btnRefreshOrders.Click += async (_, _) => await LoadOrdersAsync();
        btnUpdateStatus.Click += async (_, _) => await UpdateOrderStatusAsync();

        cmbOrderStatus.Items.AddRange(new object[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });
        cmbOrderStatus.SelectedIndex = 0;

        itemsGrid.ColumnHeaderMouseClick += (_, e) => SortGrid<ItemDto>(itemsGrid, _itemBinding, e.ColumnIndex);
        ordersGrid.ColumnHeaderMouseClick += (_, e) => SortGrid<OrderDto>(ordersGrid, _orderBinding, e.ColumnIndex);
        orderItemsGrid.ColumnHeaderMouseClick += (_, e) =>
            SortGrid<OrderItemDto>(orderItemsGrid, _orderItemsBinding, e.ColumnIndex);

        Load += async (_, _) =>
        {
            await LoadItemsAsync();
            await LoadOrdersAsync();
        };

        // Initialize reports controls
        lblReportsPlaceholder.Visible = false;
        lblTotalRevenue = new Label { Text = "Total Revenue: ", Location = new Point(16, 20), AutoSize = true };
        lblTotalOrders = new Label { Text = "Total Orders: ", Location = new Point(16, 50), AutoSize = true };
        var lblTopItems = new Label { Text = "Top Selling Items:", Location = new Point(16, 80), AutoSize = true };
        lstTopItems = new ListBox { Location = new Point(16, 100), Size = new Size(300, 120) };

        tabReports.Controls.Add(lblTotalRevenue);
        tabReports.Controls.Add(lblTotalOrders);
        tabReports.Controls.Add(lblTopItems);
        tabReports.Controls.Add(lstTopItems);

        // Enable and wire reports button
        btnGenerateSales.Enabled = true;
        btnGenerateSales.Click += async (_, _) => await GenerateSalesReportAsync();
    }

    // Fetch items from the API and bind them to the inventory grid.
    private async Task LoadItemsAsync()
    {
        var result = await _apiClient.GetItemsAsync();
        if (!result.Success)
        {
            ShowError("Failed to load items.", result.Error);
            return;
        }

        _itemBinding.DataSource = result.Data ?? new List<ItemDto>();
    }

    // Populate the item edit fields from the selected inventory row.
    private void PopulateItemFields()
    {
        if (itemsGrid.CurrentRow?.DataBoundItem is not ItemDto item)
        {
            txtItemName.Text = string.Empty;
            txtItemCategory.Text = string.Empty;
            numItemQuantity.Value = 0;
            numItemPrice.Value = 0;
            return;
        }

        txtItemName.Text = item.Name;
        txtItemCategory.Text = item.Category;
        numItemQuantity.Value = item.Quantity;
        numItemPrice.Value = Convert.ToDecimal(item.Price);
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

        await LoadItemsAsync();
    }

    // Update the selected item using the current form inputs.
    private async Task UpdateItemAsync()
    {
        if (itemsGrid.CurrentRow?.DataBoundItem is not ItemDto item)
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

        await LoadItemsAsync();
    }

    // Delete the selected item after user confirmation.
    private async Task DeleteItemAsync()
    {
        if (itemsGrid.CurrentRow?.DataBoundItem is not ItemDto item)
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

        await LoadItemsAsync();
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

    // Fetch orders from the API and bind them to the orders grid.
    private async Task LoadOrdersAsync()
    {
        var result = await _apiClient.GetOrdersAsync();
        if (!result.Success)
        {
            ShowError("Failed to load orders.", result.Error);
            return;
        }

        _orderBinding.DataSource = result.Data ?? new List<OrderDto>();
        PopulateOrderItems();
    }

    // Populate order items grid and status selection from the current order.
    private void PopulateOrderItems()
    {
        if (ordersGrid.CurrentRow?.DataBoundItem is not OrderDto order)
        {
            _orderItemsBinding.DataSource = new List<OrderItemDto>();
            return;
        }

        _orderItemsBinding.DataSource = order.OrderItems;
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

        await LoadOrdersAsync();
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

    private static void ShowError(string title, string error)
    {
        MessageBox.Show(error, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    
    private void ResizeGrids()
    {
        int rightLimit = btnRefreshOrders.Left - 12; 

        ordersGrid.Width = rightLimit - ordersGrid.Left;
        orderItemsGrid.Width = rightLimit - orderItemsGrid.Left;
    }
}