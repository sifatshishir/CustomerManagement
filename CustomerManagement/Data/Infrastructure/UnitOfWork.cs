namespace CustomerManagement.Data.Infrastructure;

using System;
using System.Data;
using CustomerManagement.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;
    
    private ICustomerRepository? _customers;
    private IInvoiceRepository? _invoices;
    private IPaymentRepository? _payments;

    public UnitOfWork(DbConnectionFactory connectionFactory)
    {
        _connection = connectionFactory.CreateConnection();
    }

    public IDbConnection Connection 
    {
        get 
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            return _connection;
        }
    }

    public IDbTransaction? Transaction => _transaction;

    public ICustomerRepository Customers => _customers ??= new CustomerRepository(this);
    public IInvoiceRepository Invoices => _invoices ??= new InvoiceRepository(this);
    public IPaymentRepository Payments => _payments ??= new PaymentRepository(this);

    public IDbTransaction BeginTransaction()
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already in progress");

        if (_connection.State != ConnectionState.Open)
            _connection.Open();

        _transaction = _connection.BeginTransaction();
        return _transaction;
    }

    public void Commit()
    {
        if (_transaction == null) return;

        try
        {
            _transaction.Commit();
        }
        catch
        {
            _transaction.Rollback();
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public void Rollback()
    {
        if (_transaction == null) return;

        try
        {
            _transaction.Rollback();
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                if (_connection != null)
                {
                    if (_connection.State != ConnectionState.Closed)
                        _connection.Close();
                    _connection.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
