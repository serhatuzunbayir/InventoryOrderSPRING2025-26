namespace DesktopApp;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private TabControl tabMain;
    private TabPage tabInventory;
    private TabPage tabOrders;
    private TabPage tabReports;
    private System.Windows.Forms.TabPage tabOptions;

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

    private Label lblLowStockThreshold;
    private NumericUpDown numLowStockThreshold;
    private Label lblPollingRate;
    private NumericUpDown numPollingRate;
    private System.Windows.Forms.Button btnSaveOptions;
    private System.Windows.Forms.Button btnManualBackup;
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
        lblReportsPlaceholder = new System.Windows.Forms.Label();
        btnGenerateSales = new System.Windows.Forms.Button();
        btnGenerateInventory = new System.Windows.Forms.Button();
        tabOptions = new System.Windows.Forms.TabPage();
        lblLowStockThreshold = new System.Windows.Forms.Label();
        numLowStockThreshold = new System.Windows.Forms.NumericUpDown();
        lblPollingRate = new System.Windows.Forms.Label();
        numPollingRate = new System.Windows.Forms.NumericUpDown();
        btnSaveOptions = new System.Windows.Forms.Button();
        btnManualBackup = new System.Windows.Forms.Button();
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
        tabInventory.Controls.Add(itemsGrid);
        tabInventory.Controls.Add(grpItemDetails);
        tabInventory.Location = new System.Drawing.Point(4, 24);
        tabInventory.Name = "tabInventory";
        tabInventory.Size = new System.Drawing.Size(942, 430);
        tabInventory.TabIndex = 0;
        tabInventory.Text = "Inventory";
        tabInventory.UseVisualStyleBackColor = true;
        // 
        // itemsGrid
        // 
        itemsGrid.Location = new System.Drawing.Point(12, 12);
        itemsGrid.MultiSelect = false;
        itemsGrid.Name = "itemsGrid";
        itemsGrid.ReadOnly = true;
        itemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        itemsGrid.Size = new System.Drawing.Size(620, 360);
        itemsGrid.TabIndex = 0;
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
        lblItemPrice.Size = new System.Drawing.Size(33, 15);
        lblItemPrice.TabIndex = 6;
        lblItemPrice.Text = "Price";
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
        tabReports.Controls.Add(lblReportsPlaceholder);
        tabReports.Controls.Add(btnGenerateSales);
        tabReports.Controls.Add(btnGenerateInventory);
        tabReports.Location = new System.Drawing.Point(4, 24);
        tabReports.Name = "tabReports";
        tabReports.Size = new System.Drawing.Size(942, 430);
        tabReports.TabIndex = 2;
        tabReports.Text = "Reports";
        tabReports.UseVisualStyleBackColor = true;
        // 
        // lblReportsPlaceholder
        // 
        lblReportsPlaceholder.AutoSize = true;
        lblReportsPlaceholder.Location = new System.Drawing.Point(16, 20);
        lblReportsPlaceholder.Name = "lblReportsPlaceholder";
        lblReportsPlaceholder.Size = new System.Drawing.Size(233, 15);
        lblReportsPlaceholder.TabIndex = 0;
        lblReportsPlaceholder.Text = "Reports will be available in a future update.";
        // 
        // btnGenerateSales
        // 
        btnGenerateSales.Enabled = false;
        btnGenerateSales.Location = new System.Drawing.Point(16, 240);
        btnGenerateSales.Name = "btnGenerateSales";
        btnGenerateSales.Size = new System.Drawing.Size(180, 28);
        btnGenerateSales.TabIndex = 1;
        btnGenerateSales.Text = "Generate Sales Report";
        // 
        // btnGenerateInventory
        // 
        btnGenerateInventory.Enabled = false;
        btnGenerateInventory.Location = new System.Drawing.Point(16, 280);
        btnGenerateInventory.Name = "btnGenerateInventory";
        btnGenerateInventory.Size = new System.Drawing.Size(180, 28);
        btnGenerateInventory.TabIndex = 2;
        btnGenerateInventory.Text = "Generate Inventory Report";
        // 
        // tabOptions
        // 
        tabOptions.Controls.Add(lblLowStockThreshold);
        tabOptions.Controls.Add(numLowStockThreshold);
        tabOptions.Controls.Add(lblPollingRate);
        tabOptions.Controls.Add(numPollingRate);
        tabOptions.Controls.Add(btnSaveOptions);
        tabOptions.Controls.Add(btnManualBackup);
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
        // lblOptionsInfo
        // 
        lblOptionsInfo.AutoSize = true;
        lblOptionsInfo.Location = new System.Drawing.Point(20, 195);
        lblOptionsInfo.Name = "lblOptionsInfo";
        lblOptionsInfo.Size = new System.Drawing.Size(113, 15);
        lblOptionsInfo.TabIndex = 6;
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
