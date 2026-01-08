using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CustomerManagement.Data.Infrastructure;
using CustomerManagement.Models;

namespace CustomerManagement.Data;

public interface IPaymentService
{
    BindingList<Payment> GetAll();
    BindingList<Payment> GetByInvoiceId(int invoiceId);
    void Add(Payment payment);
    void Update(Payment payment);
    void Delete(int id);
    Payment? GetById(int id);
}

public class PaymentService : IPaymentService
{
    private readonly Func<IUnitOfWork> _uowFactory;

    public PaymentService(Func<IUnitOfWork> uowFactory)
    {
        _uowFactory = uowFactory;
    }

    public BindingList<Payment> GetAll()
    {
        using var uow = _uowFactory();
        var list = uow.Payments.GetAll().ToList();
        return new BindingList<Payment>(list);
    }

    public BindingList<Payment> GetByInvoiceId(int invoiceId)
    {
        using var uow = _uowFactory();
        var list = uow.Payments.GetByInvoiceId(invoiceId).ToList();
        return new BindingList<Payment>(list);
    }

    public void Add(Payment payment)
    {
        if (payment == null) throw new ArgumentNullException(nameof(payment));

        using var uow = _uowFactory();
        try
        {
            uow.Payments.Add(payment);
            uow.Commit();
        }
        catch (Exception ex)
        {
            uow.Rollback();
            throw new ServiceException("Error adding payment", ex);
        }
    }

    public void Update(Payment payment)
    {
        if (payment == null) throw new ArgumentNullException(nameof(payment));

        using var uow = _uowFactory();
        try
        {
            uow.Payments.Update(payment);
            uow.Commit();
        }
        catch (Exception ex)
        {
            uow.Rollback();
            throw new ServiceException("Error updating payment", ex);
        }
    }

    public void Delete(int id)
    {
        using var uow = _uowFactory();
        try
        {
            uow.Payments.Delete(id);
            uow.Commit();
        }
        catch (Exception ex)
        {
            uow.Rollback();
            throw new ServiceException("Error deleting payment", ex);
        }
    }

    public Payment? GetById(int id)
    {
        using var uow = _uowFactory();
        return uow.Payments.GetById(id);
    }
}
