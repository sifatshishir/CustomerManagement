using System;
using System.Windows.Forms;
using CustomerManagement.Forms;
using CustomerManagement.Data;
using CustomerManagement.Data.Infrastructure;

namespace CustomerManagement.Data.Infrastructure;

public static class ApplicationBootstrap
{
    public static void Initialize()
    {
        ApplicationConfiguration.Initialize();
        SetupExceptionHandling();
    }

    private static void SetupExceptionHandling()
    {
        Application.ThreadException += (sender, e) => HandleException(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleException(e.ExceptionObject as Exception);
    }

    public static void Run()
    {
        try 
        {
            var (services, mode) = ConfigureServices();
            
            var mainForm = new MainForm(
                services.CustomerService, 
                services.InvoiceService, 
                services.PaymentService);

            // Inform user about the mode if they care, or just set title
            mainForm.Text += $" - Mode: {mode}";

            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private static ( (ICustomerService CustomerService, IInvoiceService InvoiceService, IPaymentService PaymentService) Services, string Mode) ConfigureServices()
    {
        string connectionString = "Server=localhost;Database=customermanagement;Uid=root;Pwd=123456;";
        bool dbAvailable = TestConnection(connectionString);

        if (dbAvailable)
        {
            var dbFactory = new DbConnectionFactory(connectionString);
            Func<IUnitOfWork> uowFactory = () => new UnitOfWork(dbFactory);
            
            var services = (
                (ICustomerService)new CustomerService(uowFactory),
                (IInvoiceService)new InvoiceService(uowFactory),
                (IPaymentService)new PaymentService(uowFactory)
            );
            return (services, "Database (MySQL)");
        }
        else
        {
            // Fallback to JSON
            var memoryRepo = new InMemoryCustomerRepository(); 
            Func<IUnitOfWork> uowFactory = () => new JsonUnitOfWork(memoryRepo);
            
            var services = (
                (ICustomerService)new CustomerService(uowFactory),
                (IInvoiceService)new InvoiceService(uowFactory),
                (IPaymentService)new PaymentService(uowFactory)
            );
            return (services, "In-Memory (JSON Fallback)");
        }
    }

    private static bool TestConnection(string connectionString)
    {
        try
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            conn.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void HandleException(Exception? ex)
    {
        if (ex == null) return;

        string message = "An unexpected error occurred.\n\n" + ex.Message;
        if (ex is DataAccessException)
        {
            message = "A database error occurred. Please check your connection and try again.\n\nDetails: " + ex.Message;
        }

        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
