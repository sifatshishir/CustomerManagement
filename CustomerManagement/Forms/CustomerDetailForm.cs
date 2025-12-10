namespace CustomerManagement.Forms;

using System;
using System.ComponentModel;
using System.Windows.Forms;
using CustomerManagement.Models;

public partial class CustomerDetailForm : Form
{
    private readonly bool _isNew;

    public Customer Customer { get; private set; }

    public CustomerDetailForm(Customer customer, bool isNew = false)
    {
        InitializeComponent();
        _isNew = isNew;
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        StartPosition = FormStartPosition.CenterParent;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        txtFirstName.Text = Customer.FirstName;
        txtLastName.Text = Customer.LastName;
        numAge.Value = Math.Max(1, Math.Min(120, Customer.Age));
        txtType.Text = Customer.Type;
        txtFirstName.Focus();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        errorProvider1.Clear();

        var valid = true;

        if (string.IsNullOrWhiteSpace(txtFirstName.Text))
        {
            errorProvider1.SetError(txtFirstName, "First name is required.");
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(txtLastName.Text))
        {
            errorProvider1.SetError(txtLastName, "Last name is required.");
            valid = false;
        }

        if (numAge.Value <= 0 || numAge.Value > 120)
        {
            errorProvider1.SetError(numAge, "Age must be between 1 and 120.");
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(txtType.Text))
        {
            errorProvider1.SetError(txtType, "Type is required.");
            valid = false;
        }

        if (!valid) return;

        Customer.FirstName = txtFirstName.Text.Trim();
        Customer.LastName = txtLastName.Text.Trim();
        Customer.Age = (int)numAge.Value;
        Customer.Type = txtType.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}