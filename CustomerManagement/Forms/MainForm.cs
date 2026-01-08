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
        private readonly BindingSource _bindingSource = new();

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

        _bindingSource.DataSource = _customerService.GetAll();
        dataGridViewCustomers.AutoGenerateColumns = false;
        dataGridViewCustomers.DataSource = _bindingSource;

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
        if (_bindingSource.Current is not Customer selected)
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

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (_bindingSource.Current is not Customer selected)
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
        _bindingSource.DataSource = new BindingList<Customer>(sorted);
    }

    private void btnRefresh_Click(object sender, EventArgs e) => RefreshView();

    private void RefreshView()
    {
        _bindingSource.DataSource = _customerService.GetAll();
        dataGridViewCustomers.Refresh();
    }

    private void chkAscending_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}


