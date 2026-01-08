namespace CustomerManagement.Data;

using System;
using System.Collections.Generic;
using System.Data;
using CustomerManagement.Data.Infrastructure;
using CustomerManagement.Models;
using MySql.Data.MySqlClient;

public class InvoiceRepository : BaseRepository, IInvoiceRepository
{
    public InvoiceRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public void Add(Invoice invoice)
    {
        const string sql = @"
            INSERT INTO Invoices (CustomerId, Date, TotalAmount, Status) 
            VALUES (@CustomerId, @Date, @TotalAmount, @Status);
            SELECT LAST_INSERT_ID();";

        var id = ExecuteScalar<int>(sql, new 
        { 
            invoice.CustomerId, 
            invoice.Date, 
            invoice.TotalAmount, 
            invoice.Status 
        });
        invoice.Id = id;
    }

    public void Delete(int id)
    {
        const string sql = "DELETE FROM Invoices WHERE Id = @Id";
        ExecuteNonQuery(sql, new { Id = id });
    }

    public IEnumerable<Invoice> GetAll()
    {
        const string sql = "SELECT * FROM Invoices";
        return ExecuteReader(sql, Map);
    }

    public IEnumerable<Invoice> GetByCustomerId(int customerId)
    {
        const string sql = "SELECT * FROM Invoices WHERE CustomerId = @CustomerId";
        return ExecuteReader(sql, Map, new { CustomerId = customerId });
    }

    public Invoice? GetById(int id)
    {
        const string sql = "SELECT * FROM Invoices WHERE Id = @Id";
        return ExecuteSingle(sql, Map, new { Id = id });
    }

    public void Update(Invoice invoice)
    {
        const string sql = @"
            UPDATE Invoices 
            SET CustomerId = @CustomerId, Date = @Date, TotalAmount = @TotalAmount, Status = @Status 
            WHERE Id = @Id";

        ExecuteNonQuery(sql, new 
        { 
            invoice.CustomerId, 
            invoice.Date, 
            invoice.TotalAmount, 
            invoice.Status, 
            invoice.Id 
        });
    }

    private static Invoice Map(IDataReader reader)
    {
        return new Invoice
        {
            Id = Convert.ToInt32(reader["Id"]),
            CustomerId = Convert.ToInt32(reader["CustomerId"]),
            Date = Convert.ToDateTime(reader["Date"]),
            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
            Status = reader["Status"]?.ToString() ?? string.Empty
        };
    }
}
