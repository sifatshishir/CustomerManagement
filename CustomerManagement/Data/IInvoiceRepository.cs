namespace CustomerManagement.Data;

using System.Collections.Generic;
using CustomerManagement.Models;

public interface IInvoiceRepository
{
    IEnumerable<Invoice> GetAll();
    IEnumerable<Invoice> GetByCustomerId(int customerId);
    Invoice? GetById(int id);
    void Add(Invoice invoice);
    void Update(Invoice invoice);
    void Delete(int id);
}
