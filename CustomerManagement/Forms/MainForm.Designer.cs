namespace CustomerManagement.Forms;

using System;
using System.ComponentModel;
using System.Windows.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private DataGridView dataGridViewCustomers;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnImport;
    private ComboBox cmbSort;
    private Button btnApplySort;
    private CheckBox chkAscending;
    private Button btnRefresh;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private DataGridView dataGridViewInvoices;
    private Button btnAddInvoice;
    private Button btnEditInvoice;
    private Button btnDeleteInvoice;
    private Label lblInvoices;

    private void InitializeComponent()
    {
        dataGridViewCustomers = new DataGridView();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnImport = new Button();
        cmbSort = new ComboBox();
        btnApplySort = new Button();
        chkAscending = new CheckBox();
        btnRefresh = new Button();
        
        dataGridViewInvoices = new DataGridView();
        btnAddInvoice = new Button();
        btnEditInvoice = new Button();
        btnDeleteInvoice = new Button();
        lblInvoices = new Label();

        ((ISupportInitialize)dataGridViewCustomers).BeginInit();
        ((ISupportInitialize)dataGridViewInvoices).BeginInit();
        SuspendLayout();

        // btnAdd ... (existing code, I'll update the whole block for clarity or use multi_replace if better)
        // I'll replace the whole InitializeComponent for safety since I'm changing many positions.
        
        btnAdd.Location = new Point(12, 12);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(75, 25);
        btnAdd.Text = "Add";
        btnAdd.Click += btnAdd_Click;

        btnEdit.Location = new Point(93, 12);
        btnEdit.Size = new Size(75, 25);
        btnEdit.Text = "Edit";
        btnEdit.Click += btnEdit_Click;

        btnDelete.Location = new Point(174, 12);
        btnDelete.Size = new Size(75, 25);
        btnDelete.Text = "Delete";
        btnDelete.Click += btnDelete_Click;

        btnImport.Location = new Point(255, 12);
        btnImport.Size = new Size(90, 25);
        btnImport.Text = "Import JSON";
        btnImport.Click += btnImport_Click;

        btnRefresh.Location = new Point(351, 12);
        btnRefresh.Size = new Size(75, 25);
        btnRefresh.Text = "Refresh";
        btnRefresh.Click += btnRefresh_Click;

        cmbSort.Location = new Point(451, 14);
        cmbSort.Size = new Size(140, 23);
        cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;

        chkAscending.Location = new Point(609, 16);
        chkAscending.Size = new Size(89, 19);
        chkAscending.Text = "Ascending";
        chkAscending.Checked = true;

        btnApplySort.Location = new Point(704, 12);
        btnApplySort.Size = new Size(60, 25);
        btnApplySort.Text = "Sort";
        btnApplySort.Click += btnApplySort_Click;

        // dataGridViewCustomers
        dataGridViewCustomers.AllowUserToAddRows = false;
        dataGridViewCustomers.AllowUserToDeleteRows = false;
        dataGridViewCustomers.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dataGridViewCustomers.Location = new Point(12, 50);
        dataGridViewCustomers.MultiSelect = false;
        dataGridViewCustomers.ReadOnly = true;
        dataGridViewCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewCustomers.Size = new Size(760, 200);
        dataGridViewCustomers.SelectionChanged += DataGridViewCustomers_SelectionChanged;

        // lblInvoices
        lblInvoices.AutoSize = true;
        lblInvoices.Location = new Point(12, 265);
        lblInvoices.Text = "Customer Invoices:";
        lblInvoices.Font = new Font(Font, FontStyle.Bold);

        // btnAddInvoice
        btnAddInvoice.Location = new Point(12, 290);
        btnAddInvoice.Size = new Size(90, 25);
        btnAddInvoice.Text = "New Invoice";
        btnAddInvoice.Click += btnAddInvoice_Click;

        // btnEditInvoice
        btnEditInvoice.Location = new Point(108, 290);
        btnEditInvoice.Size = new Size(75, 25);
        btnEditInvoice.Text = "Edit";
        btnEditInvoice.Click += btnEditInvoice_Click;

        // btnDeleteInvoice
        btnDeleteInvoice.Location = new Point(189, 290);
        btnDeleteInvoice.Size = new Size(75, 25);
        btnDeleteInvoice.Text = "Delete";
        btnDeleteInvoice.Click += btnDeleteInvoice_Click;

        // dataGridViewInvoices
        dataGridViewInvoices.AllowUserToAddRows = false;
        dataGridViewInvoices.AllowUserToDeleteRows = false;
        dataGridViewInvoices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataGridViewInvoices.Location = new Point(12, 325);
        dataGridViewInvoices.MultiSelect = false;
        dataGridViewInvoices.ReadOnly = true;
        dataGridViewInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewInvoices.Size = new Size(760, 150);

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 487);
        Controls.Add(dataGridViewCustomers);
        Controls.Add(btnAdd);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
        Controls.Add(btnImport);
        Controls.Add(btnRefresh);
        Controls.Add(cmbSort);
        Controls.Add(chkAscending);
        Controls.Add(btnApplySort);
        Controls.Add(lblInvoices);
        Controls.Add(btnAddInvoice);
        Controls.Add(btnEditInvoice);
        Controls.Add(btnDeleteInvoice);
        Controls.Add(dataGridViewInvoices);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Customer Management";
        ((ISupportInitialize)dataGridViewCustomers).EndInit();
        ((ISupportInitialize)dataGridViewInvoices).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}


