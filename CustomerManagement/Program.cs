namespace CustomerManagement
{
    using CustomerManagement.Data;
    using CustomerManagement.Data.Infrastructure;
    using CustomerManagement.Forms;

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationBootstrap.Initialize();
            ApplicationBootstrap.Run();
        }
    }
}