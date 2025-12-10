# Customer Management Application

A modern Windows Forms application built with **.NET 10** for managing customer data with CRUD operations, JSON import/export, and sorting capabilities.

## 📋 Features

### Core Functionality
- ✅ **Add Customers** – Create new customer records via modal dialog
- ✅ **Edit Customers** – Modify existing customer details with validation
- ✅ **Delete Customers** – Remove customers with confirmation
- ✅ **View Customers** – Display all customers in a DataGridView
- ✅ **Persistent Storage** – Save and load customer data from `customers.json`

### Data Management
- 📁 **JSON Import** – Import customer data from custom JSON files
- 💾 **Auto-Save** – Changes automatically persist to `customers.json`
- 🔄 **Refresh** – Reload data from the JSON file
- 📊 **Sorting** – Sort customers by Name, Age, or Type (ascending/descending)

### Data Validation
- 🛡️ **Input Validation** – All fields validated before saving:
  - First Name and Last Name are required
  - Age must be between 1 and 120
  - Type is required
- ⚠️ **Error Handling** – User-friendly error messages with visual indicators
- 📋 **Import Validation** – Invalid rows reported with line numbers and reasons

### UI/UX
- 🎯 **Master-Detail Pattern** – Main list with detail dialog
- 🖱️ **Double-Click Edit** – Quick edit by double-clicking a row
- 🔍 **Real-Time Updates** – Grid refreshes immediately after operations
- 🎨 **Clean Interface** – Intuitive button layout and clear column headers

---

## 🚀 Quick Start

### Option 1: Run the Executable
1. Download the latest `CustomerManagement.exe` from the [Releases](releases) page
2. Run the executable directly—no installation required
3. The app will create `customers.json` automatically on first run

### Option 2: Run from Visual Studio
1. Clone the repository:
2. Open `CustomerManagement.sln` in Visual Studio 2026
3. Press **F5** to build and run

### Option 3: Build from Source
1. Clone the repository (see Option 2)
2. Build the solution:
3. Run the application:

---

## 🎮 How to Use

### Adding a Customer
1. Click the **"Add"** button
2. Fill in the customer details:
   - First Name (required)
   - Last Name (required)
   - Age (1-120, required)
   - Type (required, e.g., "Retail", "Wholesale")
3. Click **"OK"** to save or **"Cancel"** to discard
4. Customer is automatically saved to `customers.json`

### Editing a Customer
**Method 1:**
1. Select a customer from the list
2. Click the **"Edit"** button
3. Modify the details and click **"OK"**

**Method 2:**
- Double-click any customer row to open the edit dialog

### Deleting a Customer
1. Select a customer from the list
2. Click the **"Delete"** button
3. Confirm the deletion when prompted
4. Customer is removed and changes are saved

### Importing Customers
1. Prepare a JSON file with customer data (see [JSON Format](#-json-format))
2. Click the **"Import JSON"** button
3. Select your JSON file
4. Invalid rows are reported; valid rows are imported
5. All imported customers are saved to `customers.json`

### Sorting Customers
1. Select a sort criteria from the dropdown: **"Name"**, **"Age"**, or **"Type"**
2. Choose sort order with the **"Ascending"** checkbox
3. Click **"Sort"** to apply
4. Click **"Refresh"** to return to original order

---

## ⚙️ Technical Details

### Technology Stack
- **Framework:** .NET 10
- **UI Framework:** Windows Forms
- **Data Format:** JSON (System.Text.Json)
- **Language:** C# 14.0

### Architecture
- **Repository Pattern** – Abstracted data access via `ICustomerRepository`
- **Data Binding** – `BindingList<T>` and `BindingSource` for dynamic UI updates
- **Validation Layer** – Input validation in both UI and import logic
- **Error Handling** – Try-catch blocks with user-friendly messages

### Data Flow
1. **Load** – On startup, `InMemoryCustomerRepository` loads data from `customers.json`
2. **Edit** – Users modify data via `CustomerDetailForm` with validation
3. **Save** – Changes are saved to `customers.json` via `SaveToJson()`
4. **Refresh** – UI updates via `BindingSource` and `DataGridView`

---

## 🛡️ Validation Rules

| Field | Rules |
|-------|-------|
| First Name | Required, non-empty string |
| Last Name | Required, non-empty string |
| Age | Integer between 1 and 120 |
| Type | Required, non-empty string |

---

## 🔴 Error Handling

The application handles the following scenarios:

- **File Not Found** – Missing `customers.json` is automatically created
- **Invalid JSON** – Gracefully reverts to empty customer list
- **Missing Fields** – Invalid import rows are skipped with detailed error messages
- **Out-of-Range Age** – Validation error with message
- **Empty Fields** – Cannot save without filling all fields

---

## 📦 Creating an Executable

### Method 1: Self-Contained Executable (Recommended)

Run this command in the project directory:

```powershell
dotnet publish -c Release -r win-x64 --self-contained
```

This creates a standalone `.exe` that includes the .NET runtime. Find it at:

### Method 2: Framework-Dependent Executable

Requires .NET 10 to be installed on the target machine:

```powershell
dotnet publish -c Release -r win-x64 --framework-dependent
```

### Method 3: Create Installer (Advanced)

Use Visual Studio's **Publish** feature:
1. Right-click project → **Publish**
2. Choose **Folder** as target
3. Configure settings and publish
4. (Optional) Use a tool like **Advanced Installer** or **NSIS** to create an `.msi` installer

---

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| `customers.json` not found | The app auto-creates it on first run in the app directory |
| Import fails with JSON error | Verify JSON syntax using [JSONLint](https://jsonlint.com/) |
| Cannot edit after import | Close and reopen the app, or click **"Refresh"** |
| No data after closing app | Ensure the app didn't crash; check `customers.json` file exists |
| Age field resets on edit | Age must be numeric and between 1-120 |

---

## 📝 File Locations

- **Data File:** `customers.json` (same directory as `.exe`)
- **Logs/Temp:** No log files; all errors displayed in dialogs

---

## 🤝 Contributing

To contribute to this project:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit changes: `git commit -m "Add your feature"`
4. Push to branch: `git push origin feature/your-feature`
5. Open a Pull Request

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---

## 👨‍💼 Author

**Developed by:** [Your Name/Organization]  
**Repository:** [GitHub](https://github.com/sifatshishir/CustomerManagement)

---

## 📞 Support

For issues, feature requests, or questions:
- Open an [Issue](https://github.com/sifatshishir/CustomerManagement/issues)
- Check [Discussions](https://github.com/sifatshishir/CustomerManagement/discussions)

---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2025-12-10 | Initial release with CRUD, JSON I/O, sorting |

---

**Last Updated:** December 10, 2025  
**Status:** ✅ Production Ready