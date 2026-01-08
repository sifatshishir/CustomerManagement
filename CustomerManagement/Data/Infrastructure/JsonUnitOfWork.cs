namespace CustomerManagement.Data.Infrastructure;

using System;
using System.Data;
using CustomerManagement.Data;

public class JsonUnitOfWork : IUnitOfWork
{
    private readonly InMemoryCustomerRepository _customerRepo;
    
    // JSON mode doesn't support Invoice/Payment yet, or we could add memory repos for them too.
    // For now we return null or throw NotSupported, or just implement basics.
    // To keep it simple and compile, we'll return null for new repos or dummy implementation.
    
    public JsonUnitOfWork(InMemoryCustomerRepository customerRepo)
    {
        _customerRepo = customerRepo;
    }

    public IDbConnection Connection => throw new NotSupportedException("JSON mode does not use DB connections");
    public IDbTransaction? Transaction => null; // No real transaction object

    public ICustomerRepository Customers => _customerRepo;
    public IInvoiceRepository Invoices => throw new NotSupportedException("Invoices not supported in JSON mode yet");
    public IPaymentRepository Payments => throw new NotSupportedException("Payments not supported in JSON mode yet");

    public IDbTransaction BeginTransaction()
    {
        // Fake transaction
        return new FakeTransaction();
    }

    public void Commit()
    {
        // Crucial: This is where we actually write to disk!
        _customerRepo.SaveToJson();
    }

    public void Rollback()
    {
        // Reload from disk to "undo" changes in memory?
        _customerRepo.Reload();
    }

    public void Dispose()
    {
        // Nothing to dispose
    }

    private class FakeTransaction : IDbTransaction
    {
        public IDbConnection? Connection => null;
        public IsolationLevel IsolationLevel => IsolationLevel.Unspecified;
        public void Commit() { }
        public void Dispose() { }
        public void Rollback() { }
    }
}
