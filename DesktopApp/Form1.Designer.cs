namespace DesktopApp;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private TabControl tabMain;
    private TabPage tabInventory;
    private TabPage tabOrders;
    private System.Windows.Forms.TabPage tabReports;
    private System.Windows.Forms.TabPage tabOptions;

    private DataGridView itemsGrid;
    private Label lblItemSearch;
    private TextBox txtItemSearch;
    private Button btnClearItemSearch;
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

    private System.Windows.Forms.Button btnGenerateSales;
    private Button btnGenerateInventory;
    private Label lblTotalRevenue;
    private Label lblTotalOrders;
    private Label lblTopItems;
    private ListBox lstTopItems;
    private Label lblTrendItems;
    private ComboBox cmbTrendItems;
    private Label lblTrendChartTitle;
    private PictureBox picWeeklyTrend;

    private Label lblLowStockThreshold;
    private NumericUpDown numLowStockThreshold;
    private Label lblPollingRate;
    private NumericUpDown numPollingRate;
    private System.Windows.Forms.Button btnSaveOptions;
    private System.Windows.Forms.Button btnManualBackup;
    private System.Windows.Forms.Button btnLogout;
    private System.Windows.Forms.Label lblOptionsInfo;

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

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tabMain = new System.Windows.Forms.TabControl();
        tabInventory = new System.Windows.Forms.TabPage();
        lblItemSearch = new System.Windows.Forms.Label();
        txtItemSearch = new System.Windows.Forms.TextBox();
        btnClearItemSearch = new System.Windows.Forms.Button();
        itemsGrid = new System.Windows.Forms.DataGridView();
        grpItemDetails = new System.Windows.Forms.GroupBox();
        lblItemName = new System.Windows.Forms.Label();
        txtItemName = new System.Windows.Forms.TextBox();
        lblItemCategory = new System.Windows.Forms.Label();
        txtItemCategory = new System.Windows.Forms.TextBox();
        lblItemQuantity = new System.Windows.Forms.Label();
        numItemQuantity = new System.Windows.Forms.NumericUpDown();
        lblItemPrice = new System.Windows.Forms.Label();
        numItemPrice = new System.Windows.Forms.NumericUpDown();
        btnRefreshItems = new System.Windows.Forms.Button();
        btnAddItem = new System.Windows.Forms.Button();
        btnUpdateItem = new System.Windows.Forms.Button();
        btnDeleteItem = new System.Windows.Forms.Button();
        tabOrders = new System.Windows.Forms.TabPage();
        ordersGrid = new System.Windows.Forms.DataGridView();
        lblOrderItems = new System.Windows.Forms.Label();
        orderItemsGrid = new System.Windows.Forms.DataGridView();
        btnRefreshOrders = new System.Windows.Forms.Button();
        lblOrderStatus = new System.Windows.Forms.Label();
        cmbOrderStatus = new System.Windows.Forms.ComboBox();
        btnUpdateStatus = new System.Windows.Forms.Button();
        tabReports = new System.Windows.Forms.TabPage();
        picWeeklyTrend = new System.Windows.Forms.PictureBox();
        lblTrendChartTitle = new System.Windows.Forms.Label();
        cmbTrendItems = new System.Windows.Forms.ComboBox();
        lblTrendItems = new System.Windows.Forms.Label();
        lstTopItems = new System.Windows.Forms.ListBox();
        lblTopItems = new System.Windows.Forms.Label();
        lblTotalOrders = new System.Windows.Forms.Label();
        lblTotalRevenue = new System.Windows.Forms.Label();
        btnGenerateSales = new System.Windows.Forms.Button();
        btnGenerateInventory = new System.Windows.Forms.Button();
        tabOptions = new System.Windows.Forms.TabPage();
        lblLowStockThreshold = new System.Windows.Forms.Label();
        numLowStockThreshold = new System.Windows.Forms.NumericUpDown();
        lblPollingRate = new System.Windows.Forms.Label();
        numPollingRate = new System.Windows.Forms.NumericUpDown();
        btnSaveOptions = new System.Windows.Forms.Button();
        btnManualBackup = new System.Windows.Forms.Button();
        btnLogout = new System.Windows.Forms.Button();
        lblOptionsInfo = new System.Windows.Forms.Label();
        statusStrip = new System.Windows.Forms.StatusStrip();
        lblLoggedIn = new System.Windows.Forms.ToolStripStatusLabel();
        lblBaseUrl = new System.Windows.Forms.ToolStripStatusLabel();
        tabMain.SuspendLayout();
        tabInventory.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)itemsGrid).BeginInit();
        grpItemDetails.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numItemQuantity).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numItemPrice).BeginInit();
        tabOrders.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)ordersGrid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)orderItemsGrid).BeginInit();
        tabReports.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picWeeklyTrend).BeginInit();
        tabOptions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numLowStockThreshold).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numPollingRate).BeginInit();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // tabMain
        // 
        tabMain.Controls.Add(tabInventory);
        tabMain.Controls.Add(tabOrders);
        tabMain.Controls.Add(tabReports);
        tabMain.Controls.Add(tabOptions);
        tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
        tabMain.Location = new System.Drawing.Point(0, 0);
        tabMain.Name = "tabMain";
        tabMain.SelectedIndex = 0;
        tabMain.Size = new System.Drawing.Size(950, 458);
        tabMain.TabIndex = 0;
        // 
        // tabInventory
        // 
        tabInventory.Controls.Add(lblItemSearch);
        tabInventory.Controls.Add(txtItemSearch);
        tabInventory.Controls.Add(btnClearItemSearch);
        tabInventory.Controls.Add(itemsGrid);
        tabInventory.Controls.Add(grpItemDetails);
        tabInventory.Location = new System.Drawing.Point(4, 24);
        tabInventory.Name = "tabInventory";
        tabInventory.Size = new System.Drawing.Size(942, 430);
        tabInventory.TabIndex = 0;
        tabInventory.Text = "Inventory";
        tabInventory.UseVisualStyleBackColor = true;
        // 
        // lblItemSearch
        // 
        lblItemSearch.AutoSize = true;
        lblItemSearch.Location = new System.Drawing.Point(12, 16);
        lblItemSearch.Name = "lblItemSearch";
        lblItemSearch.Size = new System.Drawing.Size(74, 15);
        lblItemSearch.TabIndex = 0;
        lblItemSearch.Text = "Search items";
        // 
        // txtItemSearch
        // 
        txtItemSearch.Location = new System.Drawing.Point(90, 12);
        txtItemSearch.Name = "txtItemSearch";
        txtItemSearch.Size = new System.Drawing.Size(180, 23);
        txtItemSearch.TabIndex = 1;
        // 
        // btnClearItemSearch
        // 
        btnClearItemSearch.Location = new System.Drawing.Point(276, 10);
        btnClearItemSearch.Name = "btnClearItemSearch";
        btnClearItemSearch.Size = new System.Drawing.Size(75, 26);
        btnClearItemSearch.TabIndex = 2;
        btnClearItemSearch.Text = "Clear";
        btnClearItemSearch.UseVisualStyleBackColor = true;
        // 
        // itemsGrid
        // 
        itemsGrid.Location = new System.Drawing.Point(12, 44);
        itemsGrid.MultiSelect = false;
        itemsGrid.Name = "itemsGrid";
        itemsGrid.ReadOnly = true;
        itemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        itemsGrid.Size = new System.Drawing.Size(620, 328);
        itemsGrid.TabIndex = 3;
        // 
        // grpItemDetails
        // 
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
        grpItemDetails.Location = new System.Drawing.Point(648, 12);
        grpItemDetails.Name = "grpItemDetails";
        grpItemDetails.Size = new System.Drawing.Size(280, 360);
        grpItemDetails.TabIndex = 1;
        grpItemDetails.TabStop = false;
        grpItemDetails.Text = "Item Details";
        // 
        // lblItemName
        // 
        lblItemName.AutoSize = true;
        lblItemName.Location = new System.Drawing.Point(16, 32);
        lblItemName.Name = "lblItemName";
        lblItemName.Size = new System.Drawing.Size(39, 15);
        lblItemName.TabIndex = 0;
        lblItemName.Text = "Name";
        // 
        // txtItemName
        // 
        txtItemName.Location = new System.Drawing.Point(96, 28);
        txtItemName.Name = "txtItemName";
        txtItemName.Size = new System.Drawing.Size(160, 23);
        txtItemName.TabIndex = 1;
        // 
        // lblItemCategory
        // 
        lblItemCategory.AutoSize = true;
        lblItemCategory.Location = new System.Drawing.Point(16, 72);
        lblItemCategory.Name = "lblItemCategory";
        lblItemCategory.Size = new System.Drawing.Size(55, 15);
        lblItemCategory.TabIndex = 2;
        lblItemCategory.Text = "Category";
        // 
        // txtItemCategory
        // 
        txtItemCategory.Location = new System.Drawing.Point(96, 68);
        txtItemCategory.Name = "txtItemCategory";
        txtItemCategory.Size = new System.Drawing.Size(160, 23);
        txtItemCategory.TabIndex = 3;
        // 
        // lblItemQuantity
        // 
        lblItemQuantity.AutoSize = true;
        lblItemQuantity.Location = new System.Drawing.Point(16, 112);
        lblItemQuantity.Name = "lblItemQuantity";
        lblItemQuantity.Size = new System.Drawing.Size(53, 15);
        lblItemQuantity.TabIndex = 4;
        lblItemQuantity.Text = "Quantity";
        // 
        // numItemQuantity
        // 
        numItemQuantity.Location = new System.Drawing.Point(96, 108);
        numItemQuantity.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numItemQuantity.Name = "numItemQuantity";
        numItemQuantity.Size = new System.Drawing.Size(120, 23);
        numItemQuantity.TabIndex = 5;
        // 
        // lblItemPrice
        // 
        lblItemPrice.AutoSize = true;
        lblItemPrice.Location = new System.Drawing.Point(16, 152);
        lblItemPrice.Name = "lblItemPrice";
        lblItemPrice.Size = new System.Drawing.Size(50, 15);
        lblItemPrice.TabIndex = 6;
        lblItemPrice.Text = "Price ($)";
        // 
        // numItemPrice
        // 
        numItemPrice.DecimalPlaces = 2;
        numItemPrice.Location = new System.Drawing.Point(96, 148);
        numItemPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numItemPrice.Name = "numItemPrice";
        numItemPrice.Size = new System.Drawing.Size(120, 23);
        numItemPrice.TabIndex = 7;
        // 
        // btnRefreshItems
        // 
        btnRefreshItems.Location = new System.Drawing.Point(16, 200);
        btnRefreshItems.Name = "btnRefreshItems";
        btnRefreshItems.Size = new System.Drawing.Size(100, 28);
        btnRefreshItems.TabIndex = 8;
        btnRefreshItems.Text = "Refresh";
        btnRefreshItems.UseVisualStyleBackColor = true;
        // 
        // btnAddItem
        // 
        btnAddItem.Location = new System.Drawing.Point(136, 200);
        btnAddItem.Name = "btnAddItem";
        btnAddItem.Size = new System.Drawing.Size(100, 28);
        btnAddItem.TabIndex = 9;
        btnAddItem.Text = "Add";
        btnAddItem.UseVisualStyleBackColor = true;
        // 
        // btnUpdateItem
        // 
        btnUpdateItem.Location = new System.Drawing.Point(16, 240);
        btnUpdateItem.Name = "btnUpdateItem";
        btnUpdateItem.Size = new System.Drawing.Size(100, 28);
        btnUpdateItem.TabIndex = 10;
        btnUpdateItem.Text = "Update";
        btnUpdateItem.UseVisualStyleBackColor = true;
        // 
        // btnDeleteItem
        // 
        btnDeleteItem.Location = new System.Drawing.Point(136, 240);
        btnDeleteItem.Name = "btnDeleteItem";
        btnDeleteItem.Size = new System.Drawing.Size(100, 28);
        btnDeleteItem.TabIndex = 11;
        btnDeleteItem.Text = "Delete";
        btnDeleteItem.UseVisualStyleBackColor = true;
        // 
        // tabOrders
        // 
        tabOrders.Controls.Add(ordersGrid);
        tabOrders.Controls.Add(lblOrderItems);
        tabOrders.Controls.Add(orderItemsGrid);
        tabOrders.Controls.Add(btnRefreshOrders);
        tabOrders.Controls.Add(lblOrderStatus);
        tabOrders.Controls.Add(cmbOrderStatus);
        tabOrders.Controls.Add(btnUpdateStatus);
        tabOrders.Location = new System.Drawing.Point(4, 24);
        tabOrders.Name = "tabOrders";
        tabOrders.Size = new System.Drawing.Size(942, 430);
        tabOrders.TabIndex = 1;
        tabOrders.Text = "Orders";
        tabOrders.UseVisualStyleBackColor = true;
        // 
        // ordersGrid
        // 
        ordersGrid.Location = new System.Drawing.Point(12, 12);
        ordersGrid.MultiSelect = false;
        ordersGrid.Name = "ordersGrid";
        ordersGrid.ReadOnly = true;
        ordersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        ordersGrid.Size = new System.Drawing.Size(620, 220);
        ordersGrid.TabIndex = 0;
        // 
        // lblOrderItems
        // 
        lblOrderItems.AutoSize = true;
        lblOrderItems.Location = new System.Drawing.Point(12, 244);
        lblOrderItems.Name = "lblOrderItems";
        lblOrderItems.Size = new System.Drawing.Size(69, 15);
        lblOrderItems.TabIndex = 1;
        lblOrderItems.Text = "Order Items";
        // 
        // orderItemsGrid
        // 
        orderItemsGrid.Location = new System.Drawing.Point(12, 264);
        orderItemsGrid.MultiSelect = false;
        orderItemsGrid.Name = "orderItemsGrid";
        orderItemsGrid.ReadOnly = true;
        orderItemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        orderItemsGrid.Size = new System.Drawing.Size(620, 140);
        orderItemsGrid.TabIndex = 2;
        // 
        // btnRefreshOrders
        // 
        btnRefreshOrders.Location = new System.Drawing.Point(648, 12);
        btnRefreshOrders.Name = "btnRefreshOrders";
        btnRefreshOrders.Size = new System.Drawing.Size(120, 28);
        btnRefreshOrders.TabIndex = 3;
        btnRefreshOrders.Text = "Refresh";
        btnRefreshOrders.UseVisualStyleBackColor = true;
        // 
        // lblOrderStatus
        // 
        lblOrderStatus.AutoSize = true;
        lblOrderStatus.Location = new System.Drawing.Point(648, 60);
        lblOrderStatus.Name = "lblOrderStatus";
        lblOrderStatus.Size = new System.Drawing.Size(39, 15);
        lblOrderStatus.TabIndex = 4;
        lblOrderStatus.Text = "Status";
        // 
        // cmbOrderStatus
        // 
        cmbOrderStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbOrderStatus.Location = new System.Drawing.Point(648, 80);
        cmbOrderStatus.Name = "cmbOrderStatus";
        cmbOrderStatus.Size = new System.Drawing.Size(220, 23);
        cmbOrderStatus.TabIndex = 5;
        // 
        // btnUpdateStatus
        // 
        btnUpdateStatus.Location = new System.Drawing.Point(648, 120);
        btnUpdateStatus.Name = "btnUpdateStatus";
        btnUpdateStatus.Size = new System.Drawing.Size(120, 28);
        btnUpdateStatus.TabIndex = 6;
        btnUpdateStatus.Text = "Update Status";
        btnUpdateStatus.UseVisualStyleBackColor = true;
        // 
        // tabReports
        // 
        tabReports.Controls.Add(picWeeklyTrend);
        tabReports.Controls.Add(lblTrendChartTitle);
        tabReports.Controls.Add(cmbTrendItems);
        tabReports.Controls.Add(lblTrendItems);
        tabReports.Controls.Add(lstTopItems);
        tabReports.Controls.Add(lblTopItems);
        tabReports.Controls.Add(lblTotalOrders);
        tabReports.Controls.Add(lblTotalRevenue);
        tabReports.Controls.Add(btnGenerateSales);
        tabReports.Controls.Add(btnGenerateInventory);
        tabReports.Location = new System.Drawing.Point(4, 24);
        tabReports.Name = "tabReports";
        tabReports.Size = new System.Drawing.Size(942, 430);
        tabReports.TabIndex = 2;
        tabReports.Text = "Reports";
        tabReports.UseVisualStyleBackColor = true;
        // 
        // picWeeklyTrend
        // 
        picWeeklyTrend.BackColor = System.Drawing.Color.White;
        picWeeklyTrend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        picWeeklyTrend.Location = new System.Drawing.Point(340, 140);
        picWeeklyTrend.Name = "picWeeklyTrend";
        picWeeklyTrend.Size = new System.Drawing.Size(560, 230);
        picWeeklyTrend.TabIndex = 10;
        picWeeklyTrend.TabStop = false;
        // 
        // lblTrendChartTitle
        // 
        lblTrendChartTitle.AutoSize = true;
        lblTrendChartTitle.Location = new System.Drawing.Point(340, 116);
        lblTrendChartTitle.Name = "lblTrendChartTitle";
        lblTrendChartTitle.Size = new System.Drawing.Size(165, 15);
        lblTrendChartTitle.TabIndex = 9;
        lblTrendChartTitle.Text = "Weekly trend will appear here.";
        // 
        // cmbTrendItems
        // 
        cmbTrendItems.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        cmbTrendItems.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
        cmbTrendItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
        cmbTrendItems.FormattingEnabled = true;
        cmbTrendItems.Location = new System.Drawing.Point(340, 84);
        cmbTrendItems.Name = "cmbTrendItems";
        cmbTrendItems.Size = new System.Drawing.Size(250, 23);
        cmbTrendItems.TabIndex = 8;
        // 
        // lblTrendItems
        // 
        lblTrendItems.AutoSize = true;
        lblTrendItems.Location = new System.Drawing.Point(340, 60);
        lblTrendItems.Name = "lblTrendItems";
        lblTrendItems.Size = new System.Drawing.Size(65, 15);
        lblTrendItems.TabIndex = 7;
        lblTrendItems.Text = "Select Item";
        // 
        // lstTopItems
        // 
        lstTopItems.FormattingEnabled = true;
        lstTopItems.ItemHeight = 15;
        lstTopItems.Location = new System.Drawing.Point(16, 100);
        lstTopItems.Name = "lstTopItems";
        lstTopItems.Size = new System.Drawing.Size(300, 124);
        lstTopItems.TabIndex = 6;
        // 
        // lblTopItems
        // 
        lblTopItems.AutoSize = true;
        lblTopItems.Location = new System.Drawing.Point(16, 80);
        lblTopItems.Name = "lblTopItems";
        lblTopItems.Size = new System.Drawing.Size(100, 15);
        lblTopItems.TabIndex = 5;
        lblTopItems.Text = "Top Selling Items:";
        // 
        // lblTotalOrders
        // 
        lblTotalOrders.AutoSize = true;
        lblTotalOrders.Location = new System.Drawing.Point(16, 50);
        lblTotalOrders.Name = "lblTotalOrders";
        lblTotalOrders.Size = new System.Drawing.Size(77, 15);
        lblTotalOrders.TabIndex = 4;
        lblTotalOrders.Text = "Total Orders: ";
        // 
        // lblTotalRevenue
        // 
        lblTotalRevenue.AutoSize = true;
        lblTotalRevenue.Location = new System.Drawing.Point(16, 20);
        lblTotalRevenue.Name = "lblTotalRevenue";
        lblTotalRevenue.Size = new System.Drawing.Size(87, 15);
        lblTotalRevenue.TabIndex = 3;
        lblTotalRevenue.Text = "Total Revenue: ";
        // 
        // btnGenerateSales
        // 
        btnGenerateSales.Location = new System.Drawing.Point(60, 230);
        btnGenerateSales.Name = "btnGenerateSales";
        btnGenerateSales.Size = new System.Drawing.Size(220, 28);
        btnGenerateSales.TabIndex = 1;
        btnGenerateSales.Text = "Total Sales Recorded";
        // 
        // btnGenerateInventory
        // 
        btnGenerateInventory.Location = new System.Drawing.Point(340, 20);
        btnGenerateInventory.Name = "btnGenerateInventory";
        btnGenerateInventory.Size = new System.Drawing.Size(220, 28);
        btnGenerateInventory.TabIndex = 2;
        btnGenerateInventory.Text = "Generate Weekly Sale Trends";
        // 
        // tabOptions
        // 
        tabOptions.Controls.Add(lblLowStockThreshold);
        tabOptions.Controls.Add(numLowStockThreshold);
        tabOptions.Controls.Add(lblPollingRate);
        tabOptions.Controls.Add(numPollingRate);
        tabOptions.Controls.Add(btnSaveOptions);
        tabOptions.Controls.Add(btnManualBackup);
        tabOptions.Controls.Add(btnLogout);
        tabOptions.Controls.Add(lblOptionsInfo);
        tabOptions.Location = new System.Drawing.Point(4, 24);
        tabOptions.Name = "tabOptions";
        tabOptions.Size = new System.Drawing.Size(942, 430);
        tabOptions.TabIndex = 3;
        tabOptions.Text = "Options";
        tabOptions.UseVisualStyleBackColor = true;
        // 
        // lblLowStockThreshold
        // 
        lblLowStockThreshold.AutoSize = true;
        lblLowStockThreshold.Location = new System.Drawing.Point(20, 24);
        lblLowStockThreshold.Name = "lblLowStockThreshold";
        lblLowStockThreshold.Size = new System.Drawing.Size(113, 15);
        lblLowStockThreshold.TabIndex = 0;
        lblLowStockThreshold.Text = "Low stock threshold";
        // 
        // numLowStockThreshold
        // 
        numLowStockThreshold.Location = new System.Drawing.Point(20, 48);
        numLowStockThreshold.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numLowStockThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numLowStockThreshold.Name = "numLowStockThreshold";
        numLowStockThreshold.Size = new System.Drawing.Size(220, 23);
        numLowStockThreshold.TabIndex = 1;
        numLowStockThreshold.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblPollingRate
        // 
        lblPollingRate.AutoSize = true;
        lblPollingRate.Location = new System.Drawing.Point(20, 96);
        lblPollingRate.Name = "lblPollingRate";
        lblPollingRate.Size = new System.Drawing.Size(121, 15);
        lblPollingRate.TabIndex = 2;
        lblPollingRate.Text = "Polling rate (seconds)";
        // 
        // numPollingRate
        // 
        numPollingRate.Location = new System.Drawing.Point(20, 120);
        numPollingRate.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
        numPollingRate.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
        numPollingRate.Name = "numPollingRate";
        numPollingRate.Size = new System.Drawing.Size(220, 23);
        numPollingRate.TabIndex = 3;
        numPollingRate.Value = new decimal(new int[] { 5, 0, 0, 0 });
        // 
        // btnSaveOptions
        // 
        btnSaveOptions.Location = new System.Drawing.Point(61, 149);
        btnSaveOptions.Name = "btnSaveOptions";
        btnSaveOptions.Size = new System.Drawing.Size(140, 30);
        btnSaveOptions.TabIndex = 4;
        btnSaveOptions.Text = "Save Options";
        btnSaveOptions.UseVisualStyleBackColor = true;
        // 
        // btnManualBackup
        // 
        btnManualBackup.Location = new System.Drawing.Point(139, 187);
        btnManualBackup.Name = "btnManualBackup";
        btnManualBackup.Size = new System.Drawing.Size(101, 30);
        btnManualBackup.TabIndex = 5;
        btnManualBackup.Text = "Manual Backup";
        btnManualBackup.UseVisualStyleBackColor = true;
        // 
        // btnLogout
        // 
        btnLogout.Location = new System.Drawing.Point(20, 235);
        btnLogout.Name = "btnLogout";
        btnLogout.Size = new System.Drawing.Size(220, 30);
        btnLogout.TabIndex = 6;
        btnLogout.Text = "Logout";
        btnLogout.UseVisualStyleBackColor = true;
        // 
        // lblOptionsInfo
        // 
        lblOptionsInfo.AutoSize = true;
        lblOptionsInfo.Location = new System.Drawing.Point(20, 195);
        lblOptionsInfo.Name = "lblOptionsInfo";
        lblOptionsInfo.Size = new System.Drawing.Size(113, 15);
        lblOptionsInfo.TabIndex = 7;
        lblOptionsInfo.Text = "Manual Save Button";
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblLoggedIn, lblBaseUrl });
        statusStrip.Location = new System.Drawing.Point(0, 458);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new System.Drawing.Size(950, 22);
        statusStrip.TabIndex = 1;
        // 
        // lblLoggedIn
        // 
        lblLoggedIn.Name = "lblLoggedIn";
        lblLoggedIn.Size = new System.Drawing.Size(0, 17);
        // 
        // lblBaseUrl
        // 
        lblBaseUrl.Name = "lblBaseUrl";
        lblBaseUrl.Size = new System.Drawing.Size(0, 17);
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(950, 480);
        Controls.Add(tabMain);
        Controls.Add(statusStrip);
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Inventory Staff Console";
        tabMain.ResumeLayout(false);
        tabInventory.ResumeLayout(false);
        tabInventory.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)itemsGrid).EndInit();
        grpItemDetails.ResumeLayout(false);
        grpItemDetails.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numItemQuantity).EndInit();
        ((System.ComponentModel.ISupportInitialize)numItemPrice).EndInit();
        tabOrders.ResumeLayout(false);
        tabOrders.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)ordersGrid).EndInit();
        ((System.ComponentModel.ISupportInitialize)orderItemsGrid).EndInit();
        tabReports.ResumeLayout(false);
        tabReports.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)picWeeklyTrend).EndInit();
        tabOptions.ResumeLayout(false);
        tabOptions.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numLowStockThreshold).EndInit();
        ((System.ComponentModel.ISupportInitialize)numPollingRate).EndInit();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
