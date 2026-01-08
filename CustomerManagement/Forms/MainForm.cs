namespace CustomerManagement.Forms;

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CustomerManagement.Data;
using CustomerManagement.Models;

    public partial class MainForm : Form
    {
        private readonly ICustomerService _customerService;
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentService _paymentService;
        private readonly BindingSource _customerBindingSource = new();
        private readonly BindingSource _invoiceBindingSource = new();

        public MainForm(ICustomerService customerService, IInvoiceService invoiceService, IPaymentService paymentService)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        _customerBindingSource.DataSource = _customerService.GetAll();
        dataGridViewCustomers.AutoGenerateColumns = false;
        dataGridViewCustomers.DataSource = _customerBindingSource;

        dataGridViewInvoices.AutoGenerateColumns = false;
        dataGridViewInvoices.DataSource = _invoiceBindingSource;

        // Configure columns
        dataGridViewCustomers.Columns.Clear();
        dataGridViewCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.FullName),
            HeaderText = "Name",
            SortMode = DataGridViewColumnSortMode.Programmatic,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
        dataGridViewCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.Age),
            HeaderText = "Age",
            Width = 60,
            SortMode = DataGridViewColumnSortMode.Programmatic
        });
        dataGridViewCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.Type),
            HeaderText = "Type",
            Width = 120,
            SortMode = DataGridViewColumnSortMode.Programmatic
        });

        cmbSort.Items.AddRange(new[] { "Name", "Age", "Type" });
        cmbSort.SelectedIndex = 0;

        dataGridViewCustomers.CellDoubleClick += DataGridViewCustomers_CellDoubleClick;

        // Configure Invoice Columns
        dataGridViewInvoices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            DataPropertyName = nameof(Invoice.Date), 
            HeaderText = "Date", 
            Width = 120 
        });
        dataGridViewInvoices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            DataPropertyName = nameof(Invoice.TotalAmount), 
            HeaderText = "Amount", 
            Width = 100,
            DefaultCellStyle = { Format = "C2" }
        });
        dataGridViewInvoices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            DataPropertyName = nameof(Invoice.Status), 
            HeaderText = "Status", 
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
        });
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var newCustomer = new Customer();
            using var dlg = new CustomerDetailForm(newCustomer, isNew: true);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _customerService.Add(dlg.Customer);
                RefreshView();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error adding customer: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnEdit_Click(object sender, EventArgs e) => EditSelected();

    private void DataGridViewCustomers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) EditSelected();
    }

    private void EditSelected()
    {
        if (_customerBindingSource.Current is not Customer selected)
        {
            MessageBox.Show(this, "Select a customer to edit.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Work on a copy so cancel leaves original unchanged
        var copy = new Customer
        {
            Id = selected.Id,
            FirstName = selected.FirstName,
            LastName = selected.LastName,
            Age = selected.Age,
            Type = selected.Type
        };

        using var dlg = new CustomerDetailForm(copy);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _customerService.Update(dlg.Customer);
            RefreshView();
        }
    }

    private void DataGridViewCustomers_SelectionChanged(object? sender, EventArgs e)
    {
        RefreshInvoices();
    }

    private void RefreshInvoices()
    {
        if (_customerBindingSource.Current is Customer selected)
        {
            _invoiceBindingSource.DataSource = _invoiceService.GetByCustomerId(selected.Id);
        }
        else
        {
            _invoiceBindingSource.DataSource = null;
        }
    }

    private void btnAddInvoice_Click(object sender, EventArgs e)
    {
        if (_customerBindingSource.Current is not Customer selected)
        {
            MessageBox.Show("Select a customer first.", "Invoice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var newInvoice = new Invoice { CustomerId = selected.Id, Date = DateTime.Now };
        using var dlg = new InvoiceDetailForm(newInvoice, isNew: true);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _invoiceService.Add(dlg.Invoice);
            RefreshInvoices();
        }
    }

    private void btnEditInvoice_Click(object sender, EventArgs e)
    {
        if (_invoiceBindingSource.Current is not Invoice selected)
        {
            MessageBox.Show("Select an invoice to edit.", "Invoice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Work on a copy
        var copy = new Invoice
        {
            Id = selected.Id,
            CustomerId = selected.CustomerId,
            Date = selected.Date,
            TotalAmount = selected.TotalAmount,
            Status = selected.Status
        };

        using var dlg = new InvoiceDetailForm(copy);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _invoiceService.Update(dlg.Invoice);
            RefreshInvoices();
        }
    }

    private void btnDeleteInvoice_Click(object sender, EventArgs e)
    {
        if (_invoiceBindingSource.Current is not Invoice selected)
        {
            MessageBox.Show("Select an invoice to delete.", "Invoice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (MessageBox.Show("Delete this invoice?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        {
            _invoiceService.Delete(selected.Id);
            RefreshInvoices();
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (_customerBindingSource.Current is not Customer selected)
        {
            MessageBox.Show(this, "Select a customer to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var ok = MessageBox.Show(this, $"Delete {selected.FullName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (ok == DialogResult.Yes)
        {
            _customerService.Delete(selected.Id);
            RefreshView();
        }
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Import Customers from JSON",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            var result = JsonCustomerImporter.ImportFromJson(openFileDialog.FileName);

            if (!result.Success)
            {
                MessageBox.Show(
                    result.ErrorMessage ?? "Import failed.",
                    "Import Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (result.Customers.Count == 0)
            {
                MessageBox.Show(
                    "No valid customers found to import.",
                    "Import",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            _customerService.Import(result.Customers);

            if (!string.IsNullOrEmpty(result.WarningMessage))
            {
                var warningDetails = result.WarningMessage;
                if (result.ValidationErrors.Count > 0)
                {
                    warningDetails += "\n\nValidation Errors:\n" + string.Join("\n", result.ValidationErrors.Take(10));
                    if (result.ValidationErrors.Count > 10)
                    {
                        warningDetails += $"\n... and {result.ValidationErrors.Count - 10} more errors.";
                    }
                }
                MessageBox.Show(
                    warningDetails,
                    "Import Completed with Warnings",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(
                    $"Successfully imported {result.Customers.Count} customer(s).",
                    "Import Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            RefreshView();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unexpected error during import: {ex.Message}",
                "Import Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnApplySort_Click(object sender, EventArgs e)
    {
        var criteria = cmbSort.SelectedItem?.ToString() ?? "Name";
        var asc = chkAscending.Checked;

        var customers = _customerService.GetAll();
        IEnumerable<Customer> ordered = criteria switch
        {
            "Age" => customers.OrderBy(c => c.Age),
            "Type" => customers.OrderBy(c => c.Type),
            _ => customers.OrderBy(c => c.FullName)
        };
        var sorted = asc
            ? ordered.ToList()
            : ordered.Reverse().ToList();
        _customerBindingSource.DataSource = new BindingList<Customer>(sorted);
    }

    private void btnRefresh_Click(object sender, EventArgs e) => RefreshView();

    private void RefreshView()
    {
        _customerBindingSource.DataSource = _customerService.GetAll();
        dataGridViewCustomers.Refresh();
        RefreshInvoices();
    }

    private void chkAscending_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}


