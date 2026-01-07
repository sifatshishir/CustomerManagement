using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CustomerManagement.Models;

namespace CustomerManagement.Data
{
    public interface ICustomerService
    {
        BindingList<Customer> GetAll();
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        Customer? FindById(int id);
        void Import(IEnumerable<Customer> customers);
        void Save();
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public BindingList<Customer> GetAll() => new BindingList<Customer>(_repo.Customers.ToList());
        public void Add(Customer customer) => _repo.Add(customer);
        public void Update(Customer customer) => _repo.Update(customer);
        public void Delete(int id) => _repo.Delete(id);
        public Customer? FindById(int id) => _repo.FindById(id);
        public void Import(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            foreach (var customer in customers)
            {
                if (customer == null)
                    continue;
                _repo.Add(customer);
            }
        }

        public void Save()
        {
            try
            {
                if (_repo is InMemoryCustomerRepository mem)
                    mem.SaveToJson();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save customer data: {ex.Message}", ex);
            }
        }
    }
}

