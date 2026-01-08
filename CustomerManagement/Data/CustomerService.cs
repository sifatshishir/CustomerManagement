using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CustomerManagement.Data.Infrastructure;
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
    }

    public class CustomerService : ICustomerService
    {
        private readonly Func<IUnitOfWork> _uowFactory;

        public CustomerService(Func<IUnitOfWork> uowFactory)
        {
            _uowFactory = uowFactory;
        }

        public BindingList<Customer> GetAll()
        {
            using var uow = _uowFactory();
            // We convert to list to eagerly fetch everything before disposing UoW
            var list = uow.Customers.GetAll().ToList();
            return new BindingList<Customer>(list);
        }

        public void Add(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            using var uow = _uowFactory();
            try
            {
                uow.Customers.Add(customer);
                uow.Commit();
            }
            catch (Exception ex)
            {
                // Note: Rollback is handled by uow.Dispose if not committed, 
                // but explicit rollback is safer or handled in Commit() catch.
                uow.Rollback();
                throw new ServiceException("Error adding customer", ex);
            }
        }

        public void Update(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            using var uow = _uowFactory();
            try
            {
                uow.Customers.Update(customer);
                uow.Commit();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                throw new ServiceException("Error updating customer", ex);
            }
        }

        public void Delete(int id)
        {
            using var uow = _uowFactory();
            try
            {
                uow.Customers.Delete(id);
                uow.Commit();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                throw new ServiceException("Error deleting customer", ex);
            }
        }

        public Customer? FindById(int id)
        {
            try
            {
                using var uow = _uowFactory();
                return uow.Customers.FindById(id);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error finding customer {id}", ex);
            }
        }

        public void Import(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            using var uow = _uowFactory();
            using var transaction = uow.BeginTransaction();

            try
            {
                foreach (var customer in customers)
                {
                    if (customer == null) continue;
                    uow.Customers.Add(customer);
                }
                
                uow.Commit();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                throw new ServiceException("Error during import", ex);
            }
        }
    }

    public class ServiceException : Exception
    {
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
