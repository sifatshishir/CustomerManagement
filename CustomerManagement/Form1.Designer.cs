namespace CustomerManagement;

using System;
using System.ComponentModel;
using System.Windows.Forms;

partial class Form1
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

    private void InitializeComponent()
    {
        components = new Container();
        dataGridViewCustomers = new DataGridView();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnImport = new Button();
        cmbSort = new ComboBox();
        btnApplySort = new Button();
        chkAscending = new CheckBox();
        btnRefresh = new Button();

        SuspendLayout();

        // dataGridViewCustomers
        dataGridViewCustomers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataGridViewCustomers.Location = new System.Drawing.Point(12, 50);
        dataGridViewCustomers.Name = "dataGridViewCustomers";
        dataGridViewCustomers.RowTemplate.Height = 25;
        dataGridViewCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewCustomers.MultiSelect = false;
        dataGridViewCustomers.Size = new System.Drawing.Size(760, 330);
        dataGridViewCustomers.ReadOnly = true;
        dataGridViewCustomers.AllowUserToAddRows = false;
        dataGridViewCustomers.AllowUserToDeleteRows = false;
        dataGridViewCustomers.AutoGenerateColumns = false;

        // btnAdd
        btnAdd.Location = new System.Drawing.Point(12, 12);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new System.Drawing.Size(75, 25);
        btnAdd.Text = "Add";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += new EventHandler(btnAdd_Click);
        btnAdd.TabIndex = 0;

        // btnEdit
        btnEdit.Location = new System.Drawing.Point(93, 12);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new System.Drawing.Size(75, 25);
        btnEdit.Text = "Edit";
        btnEdit.UseVisualStyleBackColor = true;
        btnEdit.Click += new EventHandler(btnEdit_Click);
        btnEdit.TabIndex = 1;

        // btnDelete
        btnDelete.Location = new System.Drawing.Point(174, 12);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new System.Drawing.Size(75, 25);
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += new EventHandler(btnDelete_Click);
        btnDelete.TabIndex = 2;

        // btnImport
        btnImport.Location = new System.Drawing.Point(255, 12);
        btnImport.Name = "btnImport";
        btnImport.Size = new System.Drawing.Size(75, 25);
        btnImport.Text = "Import JSON";
        btnImport.UseVisualStyleBackColor = true;
        btnImport.Click += new EventHandler(btnImport_Click);
        btnImport.TabIndex = 3;

        // btnRefresh
        btnRefresh.Location = new System.Drawing.Point(336, 12);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new System.Drawing.Size(75, 25);
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += new EventHandler(btnRefresh_Click);
        btnRefresh.TabIndex = 4;

        // cmbSort
        cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSort.Location = new System.Drawing.Point(480, 13);
        cmbSort.Name = "cmbSort";
        cmbSort.Size = new System.Drawing.Size(140, 23);
        cmbSort.TabIndex = 5;

        // chkAscending
        chkAscending.Location = new System.Drawing.Point(626, 15);
        chkAscending.Name = "chkAscending";
        chkAscending.Size = new System.Drawing.Size(80, 19);
        chkAscending.Text = "Ascending";
        chkAscending.Checked = true;
        chkAscending.TabIndex = 6;

        // btnApplySort
        btnApplySort.Location = new System.Drawing.Point(712, 12);
        btnApplySort.Name = "btnApplySort";
        btnApplySort.Size = new System.Drawing.Size(60, 25);
        btnApplySort.Text = "Sort";
        btnApplySort.UseVisualStyleBackColor = true;
        btnApplySort.Click += new EventHandler(btnApplySort_Click);
        btnApplySort.TabIndex = 7;

        // Form1
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(784, 391);
        Controls.Add(dataGridViewCustomers);
        Controls.Add(btnAdd);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
        Controls.Add(btnImport);
        Controls.Add(btnRefresh);
        Controls.Add(cmbSort);
        Controls.Add(chkAscending);
        Controls.Add(btnApplySort);
        Name = "Form1";
        Text = "Customer Management";
        StartPosition = FormStartPosition.CenterScreen;

        ResumeLayout(false);
    }
}
