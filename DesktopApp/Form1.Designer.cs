namespace DesktopApp;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private TabControl tabMain;
    private TabPage tabInventory;
    private TabPage tabOrders;
    private TabPage tabReports;

    private DataGridView itemsGrid;
    private GroupBox grpItemDetails;
    private Label lblItemName;
    private TextBox txtItemName;
    private Label lblItemCategory;
    private TextBox txtItemCategory;
    private Label lblItemQuantity;
    private NumericUpDown numItemQuantity;
    private Label lblItemPrice;
    private NumericUpDown numItemPrice;
    private Button btnRefreshItems;
    private Button btnAddItem;
    private Button btnUpdateItem;
    private Button btnDeleteItem;

    private DataGridView ordersGrid;
    private DataGridView orderItemsGrid;
    private Button btnRefreshOrders;
    private ComboBox cmbOrderStatus;
    private Button btnUpdateStatus;
    private Label lblOrderItems;
    private Label lblOrderStatus;

    private Label lblReportsPlaceholder;
    private Button btnGenerateSales;
    private Button btnGenerateInventory;

    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblLoggedIn;
    private ToolStripStatusLabel lblBaseUrl;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tabMain = new TabControl();
        tabInventory = new TabPage();
        tabOrders = new TabPage();
        tabReports = new TabPage();

        itemsGrid = new DataGridView();
        grpItemDetails = new GroupBox();
        lblItemName = new Label();
        txtItemName = new TextBox();
        lblItemCategory = new Label();
        txtItemCategory = new TextBox();
        lblItemQuantity = new Label();
        numItemQuantity = new NumericUpDown();
        lblItemPrice = new Label();
        numItemPrice = new NumericUpDown();
        btnRefreshItems = new Button();
        btnAddItem = new Button();
        btnUpdateItem = new Button();
        btnDeleteItem = new Button();

        ordersGrid = new DataGridView();
        orderItemsGrid = new DataGridView();
        btnRefreshOrders = new Button();
        cmbOrderStatus = new ComboBox();
        btnUpdateStatus = new Button();
        lblOrderItems = new Label();
        lblOrderStatus = new Label();

        lblReportsPlaceholder = new Label();
        btnGenerateSales = new Button();
        btnGenerateInventory = new Button();

        statusStrip = new StatusStrip();
        lblLoggedIn = new ToolStripStatusLabel();
        lblBaseUrl = new ToolStripStatusLabel();

        SuspendLayout();

        tabMain.Dock = DockStyle.Fill;
        tabMain.TabPages.Add(tabInventory);
        tabMain.TabPages.Add(tabOrders);
        tabMain.TabPages.Add(tabReports);

        tabInventory.Text = "Inventory";
        tabInventory.UseVisualStyleBackColor = true;

        itemsGrid.Location = new Point(12, 12);
        itemsGrid.Name = "itemsGrid";
        itemsGrid.ReadOnly = true;
        itemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        itemsGrid.MultiSelect = false;
        itemsGrid.Size = new Size(620, 360);

        grpItemDetails.Location = new Point(648, 12);
        grpItemDetails.Size = new Size(280, 360);
        grpItemDetails.Text = "Item Details";

        lblItemName.AutoSize = true;
        lblItemName.Location = new Point(16, 32);
        lblItemName.Text = "Name";

        txtItemName.Location = new Point(96, 28);
        txtItemName.Size = new Size(160, 23);

        lblItemCategory.AutoSize = true;
        lblItemCategory.Location = new Point(16, 72);
        lblItemCategory.Text = "Category";

        txtItemCategory.Location = new Point(96, 68);
        txtItemCategory.Size = new Size(160, 23);

        lblItemQuantity.AutoSize = true;
        lblItemQuantity.Location = new Point(16, 112);
        lblItemQuantity.Text = "Quantity";

        numItemQuantity.Location = new Point(96, 108);
        numItemQuantity.Maximum = 1000000;

        lblItemPrice.AutoSize = true;
        lblItemPrice.Location = new Point(16, 152);
        lblItemPrice.Text = "Price";

        numItemPrice.Location = new Point(96, 148);
        numItemPrice.DecimalPlaces = 2;
        numItemPrice.Maximum = 100000000;

        btnRefreshItems.Location = new Point(16, 200);
        btnRefreshItems.Size = new Size(100, 28);
        btnRefreshItems.Text = "Refresh";
        btnRefreshItems.UseVisualStyleBackColor = true;

        btnAddItem.Location = new Point(136, 200);
        btnAddItem.Size = new Size(100, 28);
        btnAddItem.Text = "Add";
        btnAddItem.UseVisualStyleBackColor = true;

        btnUpdateItem.Location = new Point(16, 240);
        btnUpdateItem.Size = new Size(100, 28);
        btnUpdateItem.Text = "Update";
        btnUpdateItem.UseVisualStyleBackColor = true;

        btnDeleteItem.Location = new Point(136, 240);
        btnDeleteItem.Size = new Size(100, 28);
        btnDeleteItem.Text = "Delete";
        btnDeleteItem.UseVisualStyleBackColor = true;

        grpItemDetails.Controls.Add(lblItemName);
        grpItemDetails.Controls.Add(txtItemName);
        grpItemDetails.Controls.Add(lblItemCategory);
        grpItemDetails.Controls.Add(txtItemCategory);
        grpItemDetails.Controls.Add(lblItemQuantity);
        grpItemDetails.Controls.Add(numItemQuantity);
        grpItemDetails.Controls.Add(lblItemPrice);
        grpItemDetails.Controls.Add(numItemPrice);
        grpItemDetails.Controls.Add(btnRefreshItems);
        grpItemDetails.Controls.Add(btnAddItem);
        grpItemDetails.Controls.Add(btnUpdateItem);
        grpItemDetails.Controls.Add(btnDeleteItem);

        tabInventory.Controls.Add(itemsGrid);
        tabInventory.Controls.Add(grpItemDetails);

        tabOrders.Text = "Orders";
        tabOrders.UseVisualStyleBackColor = true;

        ordersGrid.Location = new Point(12, 12);
        ordersGrid.Name = "ordersGrid";
        ordersGrid.ReadOnly = true;
        ordersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        ordersGrid.MultiSelect = false;
        ordersGrid.Size = new Size(620, 220);

        lblOrderItems.AutoSize = true;
        lblOrderItems.Location = new Point(12, 244);
        lblOrderItems.Text = "Order Items";

        orderItemsGrid.Location = new Point(12, 264);
        orderItemsGrid.Name = "orderItemsGrid";
        orderItemsGrid.ReadOnly = true;
        orderItemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        orderItemsGrid.MultiSelect = false;
        orderItemsGrid.Size = new Size(620, 140);

        btnRefreshOrders.Location = new Point(648, 12);
        btnRefreshOrders.Size = new Size(120, 28);
        btnRefreshOrders.Text = "Refresh";
        btnRefreshOrders.UseVisualStyleBackColor = true;

        lblOrderStatus.AutoSize = true;
        lblOrderStatus.Location = new Point(648, 60);
        lblOrderStatus.Text = "Status";

        cmbOrderStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbOrderStatus.Location = new Point(648, 80);
        cmbOrderStatus.Size = new Size(220, 23);

        btnUpdateStatus.Location = new Point(648, 120);
        btnUpdateStatus.Size = new Size(120, 28);
        btnUpdateStatus.Text = "Update Status";
        btnUpdateStatus.UseVisualStyleBackColor = true;

        tabOrders.Controls.Add(ordersGrid);
        tabOrders.Controls.Add(lblOrderItems);
        tabOrders.Controls.Add(orderItemsGrid);
        tabOrders.Controls.Add(btnRefreshOrders);
        tabOrders.Controls.Add(lblOrderStatus);
        tabOrders.Controls.Add(cmbOrderStatus);
        tabOrders.Controls.Add(btnUpdateStatus);

        tabReports.Text = "Reports";
        tabReports.UseVisualStyleBackColor = true;

        lblReportsPlaceholder.AutoSize = true;
        lblReportsPlaceholder.Location = new Point(16, 20);
        lblReportsPlaceholder.Text = "Reports will be available in a future update.";

        btnGenerateSales.Location = new Point(16, 240);
        btnGenerateSales.Size = new Size(180, 28);
        btnGenerateSales.Text = "Generate Sales Report";
        btnGenerateSales.Enabled = false;

        btnGenerateInventory.Location = new Point(16, 280);
        btnGenerateInventory.Size = new Size(180, 28);
        btnGenerateInventory.Text = "Generate Inventory Report";
        btnGenerateInventory.Enabled = false;

        tabReports.Controls.Add(lblReportsPlaceholder);
        tabReports.Controls.Add(btnGenerateSales);
        tabReports.Controls.Add(btnGenerateInventory);

        statusStrip.Items.Add(lblLoggedIn);
        statusStrip.Items.Add(lblBaseUrl);
        statusStrip.Dock = DockStyle.Bottom;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(950, 480);
        Controls.Add(tabMain);
        Controls.Add(statusStrip);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Inventory Staff Console";

        ResumeLayout(false);
        PerformLayout();
    }
}