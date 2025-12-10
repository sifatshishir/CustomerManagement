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
        dataGridViewCustomers = new DataGridView();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnImport = new Button();
        cmbSort = new ComboBox();
        btnApplySort = new Button();
        chkAscending = new CheckBox();
        btnRefresh = new Button();
        ((ISupportInitialize)dataGridViewCustomers).BeginInit();
        SuspendLayout();
        // 
        // dataGridViewCustomers
        // 
        dataGridViewCustomers.AllowUserToAddRows = false;
        dataGridViewCustomers.AllowUserToDeleteRows = false;
        dataGridViewCustomers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataGridViewCustomers.Location = new Point(12, 50);
        dataGridViewCustomers.MultiSelect = false;
        dataGridViewCustomers.Name = "dataGridViewCustomers";
        dataGridViewCustomers.ReadOnly = true;
        dataGridViewCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewCustomers.Size = new Size(760, 330);
        dataGridViewCustomers.TabIndex = 0;
        // 
        // btnAdd
        // 
        btnAdd.Location = new Point(12, 12);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(75, 25);
        btnAdd.TabIndex = 0;
        btnAdd.Text = "Add";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;
        // 
        // btnEdit
        // 
        btnEdit.Location = new Point(93, 12);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(75, 25);
        btnEdit.TabIndex = 1;
        btnEdit.Text = "Edit";
        btnEdit.UseVisualStyleBackColor = true;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.Location = new Point(174, 12);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(75, 25);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnImport
        // 
        btnImport.Location = new Point(255, 12);
        btnImport.Name = "btnImport";
        btnImport.Size = new Size(75, 25);
        btnImport.TabIndex = 3;
        btnImport.Text = "Import JSON";
        btnImport.UseVisualStyleBackColor = true;
        btnImport.Click += btnImport_Click;
        // 
        // cmbSort
        // 
        cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSort.Location = new Point(451, 14);
        cmbSort.Name = "cmbSort";
        cmbSort.Size = new Size(140, 23);
        cmbSort.TabIndex = 5;
        cmbSort.SelectedIndexChanged += cmbSort_SelectedIndexChanged;
        // 
        // btnApplySort
        // 
        btnApplySort.Location = new Point(704, 12);
        btnApplySort.Name = "btnApplySort";
        btnApplySort.Size = new Size(60, 25);
        btnApplySort.TabIndex = 7;
        btnApplySort.Text = "Sort";
        btnApplySort.UseVisualStyleBackColor = true;
        btnApplySort.Click += btnApplySort_Click;
        // 
        // chkAscending
        // 
        chkAscending.Checked = true;
        chkAscending.CheckState = CheckState.Checked;
        chkAscending.Location = new Point(609, 16);
        chkAscending.Name = "chkAscending";
        chkAscending.Size = new Size(89, 19);
        chkAscending.TabIndex = 6;
        chkAscending.Text = "Ascending";
        chkAscending.CheckedChanged += chkAscending_CheckedChanged;
        // 
        // btnRefresh
        // 
        btnRefresh.Location = new Point(336, 12);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(75, 25);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 391);
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
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Customer Management";
        ((ISupportInitialize)dataGridViewCustomers).EndInit();
        ResumeLayout(false);
    }
}
