namespace CustomerManagement.Data;

using System.ComponentModel;
using CustomerManagement.Models;

public interface ICustomerRepository
{
    BindingList<Customer> Customers { get; }

    void Add(Customer customer);

    void Update(Customer customer);

    void Delete(int id);

    Customer? FindById(int id);
}