namespace CustomerManagement.Data.Infrastructure;

using System;
using System.Data;
using CustomerManagement.Data;

public interface IUnitOfWork : IDisposable
{
    IDbTransaction BeginTransaction();
    void Commit();
    void Rollback();
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }

    ICustomerRepository Customers { get; }
    IInvoiceRepository Invoices { get; }
    IPaymentRepository Payments { get; }
}
