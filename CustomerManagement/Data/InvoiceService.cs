using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CustomerManagement.Data.Infrastructure;
using CustomerManagement.Models;

namespace CustomerManagement.Data;

public interface IInvoiceService
{
    BindingList<Invoice> GetAll();
    BindingList<Invoice> GetByCustomerId(int customerId);
    void Add(Invoice invoice);
    void Update(Invoice invoice);
    void Delete(int id);
    Invoice? GetById(int id);
}

public class InvoiceService : IInvoiceService
{
    private readonly Func<IUnitOfWork> _uowFactory;

    public InvoiceService(Func<IUnitOfWork> uowFactory)
    {
        _uowFactory = uowFactory;
    }

    public BindingList<Invoice> GetAll()
    {
        using var uow = _uowFactory();
        var list = uow.Invoices.GetAll().ToList();
        return new BindingList<Invoice>(list);
    }

    public BindingList<Invoice> GetByCustomerId(int customerId)
    {
        using var uow = _uowFactory();
        var list = uow.Invoices.GetByCustomerId(customerId).ToList();
        return new BindingList<Invoice>(list);
    }

    public void Add(Invoice invoice)
    {
        if (invoice == null) throw new ArgumentNullException(nameof(invoice));

        using var uow = _uowFactory();
        try
        {
            uow.Invoices.Add(invoice);
            uow.Commit();
        }
        catch (Exception ex)
        {
            uow.Rollback();
            throw new ServiceException("Error adding invoice", ex);
        }
    }

    public void Update(Invoice invoice)
    {
        if (invoice == null) throw new ArgumentNullException(nameof(invoice));

        using var uow = _uowFactory();
        try
        {
            uow.Invoices.Update(invoice);
            uow.Commit();
        }
        catch (Exception ex)
        {
            uow.Rollback();
            throw new ServiceException("Error updating invoice", ex);
        }
    }

    public void Delete(int id)
    {
        using var uow = _uowFactory();
        try
        {
            uow.Invoices.Delete(id);
            uow.Commit();
        }
        catch (Exception ex)
        {
            uow.Rollback();
            throw new ServiceException("Error deleting invoice", ex);
        }
    }

    public Invoice? GetById(int id)
    {
        using var uow = _uowFactory();
        return uow.Invoices.GetById(id);
    }
}
