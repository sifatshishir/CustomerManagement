namespace CustomerManagement.Forms;

using System;
using System.ComponentModel;
using System.Windows.Forms;

partial class CustomerDetailForm
{
    private System.ComponentModel.IContainer components = null;
    private TextBox txtFirstName;
    private TextBox txtLastName;
    private NumericUpDown numAge;
    private TextBox txtType;
    private Button btnOk;
    private Button btnCancel;
    private Label lblFirst;
    private Label lblLast;
    private Label lblAge;
    private Label lblType;
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
        txtFirstName = new TextBox();
        txtLastName = new TextBox();
        numAge = new NumericUpDown();
        txtType = new TextBox();
        btnOk = new Button();
        btnCancel = new Button();
        lblFirst = new Label();
        lblLast = new Label();
        lblAge = new Label();
        lblType = new Label();
        errorProvider1 = new ErrorProvider(components);

        ((System.ComponentModel.ISupportInitialize)numAge).BeginInit();
        ((ISupportInitialize)errorProvider1).BeginInit();
        SuspendLayout();

        // lblFirst
        lblFirst.AutoSize = true;
        lblFirst.Location = new System.Drawing.Point(12, 15);
        lblFirst.Name = "lblFirst";
        lblFirst.Size = new System.Drawing.Size(80, 15);
        lblFirst.Text = "First Name:";
        lblFirst.TabIndex = 0;

        // txtFirstName
        txtFirstName.Location = new System.Drawing.Point(100, 12);
        txtFirstName.Name = "txtFirstName";
        txtFirstName.Size = new System.Drawing.Size(220, 23);
        txtFirstName.TabIndex = 1;

        // lblLast
        lblLast.AutoSize = true;
        lblLast.Location = new System.Drawing.Point(12, 50);
        lblLast.Name = "lblLast";
        lblLast.Size = new System.Drawing.Size(79, 15);
        lblLast.Text = "Last Name:";
        lblLast.TabIndex = 2;

        // txtLastName
        txtLastName.Location = new System.Drawing.Point(100, 47);
        txtLastName.Name = "txtLastName";
        txtLastName.Size = new System.Drawing.Size(220, 23);
        txtLastName.TabIndex = 3;

        // lblAge
        lblAge.AutoSize = true;
        lblAge.Location = new System.Drawing.Point(12, 85);
        lblAge.Name = "lblAge";
        lblAge.Size = new System.Drawing.Size(33, 15);
        lblAge.Text = "Age:";
        lblAge.TabIndex = 4;

        // numAge
        numAge.Location = new System.Drawing.Point(100, 82);
        numAge.Maximum = 120;
        numAge.Minimum = 1;
        numAge.Name = "numAge";
        numAge.Size = new System.Drawing.Size(80, 23);
        numAge.Value = 30;
        numAge.TabIndex = 5;

        // lblType
        lblType.AutoSize = true;
        lblType.Location = new System.Drawing.Point(12, 120);
        lblType.Name = "lblType";
        lblType.Size = new System.Drawing.Size(36, 15);
        lblType.Text = "Type:";
        lblType.TabIndex = 6;

        // txtType
        txtType.Location = new System.Drawing.Point(100, 117);
        txtType.Name = "txtType";
        txtType.Size = new System.Drawing.Size(220, 23);
        txtType.TabIndex = 7;

        // btnOk
        btnOk.Location = new System.Drawing.Point(164, 155);
        btnOk.Name = "btnOk";
        btnOk.Size = new System.Drawing.Size(75, 25);
        btnOk.Text = "OK";
        btnOk.UseVisualStyleBackColor = true;
        btnOk.Click += new EventHandler(btnOk_Click);
        btnOk.TabIndex = 8;

        // btnCancel
        btnCancel.Location = new System.Drawing.Point(245, 155);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new System.Drawing.Size(75, 25);
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += new EventHandler(btnCancel_Click);
        btnCancel.TabIndex = 9;

        // errorProvider1
        errorProvider1.ContainerControl = this;

        // CustomerDetailForm
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(334, 195);
        Controls.Add(lblFirst);
        Controls.Add(txtFirstName);
        Controls.Add(lblLast);
        Controls.Add(txtLastName);
        Controls.Add(lblAge);
        Controls.Add(numAge);
        Controls.Add(lblType);
        Controls.Add(txtType);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CustomerDetailForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        Text = "Customer Details";
        AcceptButton = btnOk;
        CancelButton = btnCancel;

        ((System.ComponentModel.ISupportInitialize)numAge).EndInit();
        ((ISupportInitialize)errorProvider1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}