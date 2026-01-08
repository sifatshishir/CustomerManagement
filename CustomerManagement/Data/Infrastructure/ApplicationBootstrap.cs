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
        // In a real app, this would come from appsettings.json
        bool useDatabase = true; 

        try 
        {
            var services = ConfigureServices(useDatabase);
            
            var mainForm = new MainForm(
                services.CustomerService, 
                services.InvoiceService, 
                services.PaymentService);

            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private static (ICustomerService CustomerService, IInvoiceService InvoiceService, IPaymentService PaymentService) ConfigureServices(bool useDatabase)
    {
        if (useDatabase)
        {
            string connectionString = "Server=localhost;Database=customermanagement;Uid=root;Pwd=password;";
            var dbFactory = new DbConnectionFactory(connectionString);
            Func<IUnitOfWork> uowFactory = () => new UnitOfWork(dbFactory);
            
            return (
                new CustomerService(uowFactory),
                new InvoiceService(uowFactory),
                new PaymentService(uowFactory)
            );
        }
        else
        {
            var memoryRepo = new InMemoryCustomerRepository(); 
            Func<IUnitOfWork> uowFactory = () => new JsonUnitOfWork(memoryRepo);
            
            return (
                new CustomerService(uowFactory),
                new InvoiceService(uowFactory),
                new PaymentService(uowFactory)
            );
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
