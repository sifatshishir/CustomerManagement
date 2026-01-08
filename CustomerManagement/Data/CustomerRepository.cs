namespace CustomerManagement.Data;

using System;
using System.Collections.Generic;
using System.Data;
using CustomerManagement.Data.Infrastructure;
using CustomerManagement.Models;
using MySql.Data.MySqlClient;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public void Add(Customer customer)
    {
        const string sql = @"
            INSERT INTO Customers (FirstName, LastName, Age, Type) 
            VALUES (@FirstName, @LastName, @Age, @Type);
            SELECT LAST_INSERT_ID();";

        var id = ExecuteScalar<int>(sql, new 
        { 
            customer.FirstName, 
            customer.LastName, 
            customer.Age, 
            customer.Type 
        });
        customer.Id = id;
    }

    public void Delete(int id)
    {
        const string sql = "DELETE FROM Customers WHERE Id = @Id";
        ExecuteNonQuery(sql, new { Id = id });
    }

    public Customer? FindById(int id)
    {
        const string sql = "SELECT * FROM Customers WHERE Id = @Id";
        return ExecuteSingle(sql, Map, new { Id = id });
    }

    public IEnumerable<Customer> GetAll()
    {
        const string sql = "SELECT * FROM Customers";
        return ExecuteReader(sql, Map);
    }

    public void Update(Customer customer)
    {
        const string sql = @"
            UPDATE Customers 
            SET FirstName = @FirstName, LastName = @LastName, Age = @Age, Type = @Type 
            WHERE Id = @Id";

        ExecuteNonQuery(sql, new 
        { 
            customer.FirstName, 
            customer.LastName, 
            customer.Age, 
            customer.Type, 
            customer.Id 
        });
    }

    private static Customer Map(IDataReader reader)
    {
        return new Customer
        {
            Id = Convert.ToInt32(reader["Id"]),
            FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
            LastName = reader["LastName"]?.ToString() ?? string.Empty,
            Age = Convert.ToInt32(reader["Age"]),
            Type = reader["Type"]?.ToString() ?? string.Empty
        };
    }
}
