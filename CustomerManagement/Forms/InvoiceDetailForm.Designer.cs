namespace CustomerManagement.Forms;

using System;
using System.ComponentModel;
using System.Windows.Forms;

partial class InvoiceDetailForm
{
    private IContainer components = null;
    private DateTimePicker dtpDate;
    private NumericUpDown numAmount;
    private TextBox txtStatus;
    private Button btnOk;
    private Button btnCancel;
    private Label lblDate;
    private Label lblAmount;
    private Label lblStatus;
    private ErrorProvider errorProvider1;

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
        components = new Container();
        dtpDate = new DateTimePicker();
        numAmount = new NumericUpDown();
        txtStatus = new TextBox();
        btnOk = new Button();
        btnCancel = new Button();
        lblDate = new Label();
        lblAmount = new Label();
        lblStatus = new Label();
        errorProvider1 = new ErrorProvider(components);

        ((ISupportInitialize)numAmount).BeginInit();
        ((ISupportInitialize)errorProvider1).BeginInit();
        SuspendLayout();

        // lblDate
        lblDate.AutoSize = true;
        lblDate.Location = new System.Drawing.Point(12, 15);
        lblDate.Text = "Date:";

        // dtpDate
        dtpDate.Location = new System.Drawing.Point(100, 12);
        dtpDate.Size = new System.Drawing.Size(200, 23);

        // lblAmount
        lblAmount.AutoSize = true;
        lblAmount.Location = new System.Drawing.Point(12, 50);
        lblAmount.Text = "Amount:";

        // numAmount
        numAmount.Location = new System.Drawing.Point(100, 47);
        numAmount.Maximum = 1000000;
        numAmount.DecimalPlaces = 2;
        numAmount.Size = new System.Drawing.Size(120, 23);

        // lblStatus
        lblStatus.AutoSize = true;
        lblStatus.Location = new System.Drawing.Point(12, 85);
        lblStatus.Text = "Status:";

        // txtStatus
        txtStatus.Location = new System.Drawing.Point(100, 82);
        txtStatus.Size = new System.Drawing.Size(200, 23);

        // btnOk
        btnOk.Location = new System.Drawing.Point(144, 125);
        btnOk.Size = new System.Drawing.Size(75, 25);
        btnOk.Text = "OK";
        btnOk.Click += new EventHandler(btnOk_Click);

        // btnCancel
        btnCancel.Location = new System.Drawing.Point(225, 125);
        btnCancel.Size = new System.Drawing.Size(75, 25);
        btnCancel.Text = "Cancel";
        btnCancel.Click += new EventHandler(btnCancel_Click);

        // errorProvider1
        errorProvider1.ContainerControl = this;

        // InvoiceDetailForm
        ClientSize = new System.Drawing.Size(320, 165);
        Controls.Add(lblDate);
        Controls.Add(dtpDate);
        Controls.Add(lblAmount);
        Controls.Add(numAmount);
        Controls.Add(lblStatus);
        Controls.Add(txtStatus);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Invoice Details";

        ((ISupportInitialize)numAmount).EndInit();
        ((ISupportInitialize)errorProvider1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
