using System;
using System.Windows.Forms;
using CustomerManagement.Models;

namespace CustomerManagement.Forms;

public partial class InvoiceDetailForm : Form
{
    public Invoice Invoice { get; private set; }

    public InvoiceDetailForm(Invoice invoice, bool isNew = false)
    {
        InitializeComponent();
        Invoice = invoice;
        Text = isNew ? "Add Invoice" : "Edit Invoice";

        dtpDate.Value = invoice.Date == default ? DateTime.Now : invoice.Date;
        numAmount.Value = invoice.TotalAmount;
        txtStatus.Text = invoice.Status;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (numAmount.Value <= 0)
        {
            errorProvider1.SetError(numAmount, "Amount must be greater than zero.");
            return;
        }

        Invoice.Date = dtpDate.Value;
        Invoice.TotalAmount = numAmount.Value;
        Invoice.Status = txtStatus.Text;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
