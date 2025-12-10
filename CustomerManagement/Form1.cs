namespace CustomerManagement;

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CustomerManagement.Data;
using CustomerManagement.Forms;
using CustomerManagement.Models;

public partial class Form1 : Form
{
    private readonly InMemoryCustomerRepository _repo = new();
    private readonly BindingSource _bindingSource = new();

    public Form1()
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterScreen;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        _bindingSource.DataSource = _repo.Customers;
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
        var newCustomer = new Customer();
        using var dlg = new CustomerDetailForm(newCustomer, isNew: true);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _repo.Add(dlg.Customer);
            _repo.SaveToJson();
            RefreshView();
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
            _repo.Update(dlg.Customer);
            _repo.SaveToJson();
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
            _repo.Delete(selected.Id);
            _repo.SaveToJson();
            RefreshView();
        }
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Import Customers from JSON",
            InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
        };

        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
            var importedCustomers = JsonCustomerImporter.ImportFromJson(openFileDialog.FileName);
            foreach (var customer in importedCustomers)
            {
                _repo.Add(customer);
            }

            if (importedCustomers.Count > 0)
            {
                _repo.SaveToJson();
            }

            RefreshView();
        }
    }

    private void btnApplySort_Click(object sender, EventArgs e)
    {
        var criteria = cmbSort.SelectedItem?.ToString() ?? "Name";
        var asc = chkAscending.Checked;

        IOrderedEnumerable<Customer> ordered = criteria switch
        {
            "Age" => _repo.Customers.OrderBy(c => c.Age),
            "Type" => _repo.Customers.OrderBy(c => c.Type),
            _ => _repo.Customers.OrderBy(c => c.FullName)
        };

        var sorted = asc
            ? ordered.ToList()
            : ordered.Reverse().ToList();

        _bindingSource.DataSource = new BindingList<Customer>(sorted);
    }

    private void btnRefresh_Click(object sender, EventArgs e) => RefreshView();

    private void RefreshView()
    {
        _bindingSource.DataSource = _repo.Customers;
        dataGridViewCustomers.Refresh();
    }
}
