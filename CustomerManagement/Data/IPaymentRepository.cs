namespace CustomerManagement.Data;

using System.Collections.Generic;
using CustomerManagement.Models;

public interface IPaymentRepository
{
    IEnumerable<Payment> GetAll();
    IEnumerable<Payment> GetByInvoiceId(int invoiceId);
    Payment? GetById(int id);
    void Add(Payment payment);
    void Update(Payment payment);
    void Delete(int id);
}
