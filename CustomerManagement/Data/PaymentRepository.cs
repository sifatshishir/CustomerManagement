namespace CustomerManagement.Data;

using System;
using System.Collections.Generic;
using System.Data;
using CustomerManagement.Data.Infrastructure;
using CustomerManagement.Models;
using MySql.Data.MySqlClient;

public class PaymentRepository : BaseRepository, IPaymentRepository
{
    public PaymentRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public void Add(Payment payment)
    {
        const string sql = @"
            INSERT INTO Payments (InvoiceId, Date, Amount, Method) 
            VALUES (@InvoiceId, @Date, @Amount, @Method);
            SELECT LAST_INSERT_ID();";

        var id = ExecuteScalar<int>(sql, new 
        { 
            payment.InvoiceId, 
            payment.Date, 
            payment.Amount, 
            payment.Method 
        });
        payment.Id = id;
    }

    public void Delete(int id)
    {
        const string sql = "DELETE FROM Payments WHERE Id = @Id";
        ExecuteNonQuery(sql, new { Id = id });
    }

    public IEnumerable<Payment> GetAll()
    {
        const string sql = "SELECT * FROM Payments";
        return ExecuteReader(sql, Map);
    }

    public IEnumerable<Payment> GetByInvoiceId(int invoiceId)
    {
        const string sql = "SELECT * FROM Payments WHERE InvoiceId = @InvoiceId";
        return ExecuteReader(sql, Map, new { InvoiceId = invoiceId });
    }

    public Payment? GetById(int id)
    {
        const string sql = "SELECT * FROM Payments WHERE Id = @Id";
        return ExecuteSingle(sql, Map, new { Id = id });
    }

    public void Update(Payment payment)
    {
        const string sql = @"
            UPDATE Payments 
            SET InvoiceId = @InvoiceId, Date = @Date, Amount = @Amount, Method = @Method 
            WHERE Id = @Id";

        ExecuteNonQuery(sql, new 
        { 
            payment.InvoiceId, 
            payment.Date, 
            payment.Amount, 
            payment.Method, 
            payment.Id 
        });
    }

    private static Payment Map(IDataReader reader)
    {
        return new Payment
        {
            Id = Convert.ToInt32(reader["Id"]),
            InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
            Date = Convert.ToDateTime(reader["Date"]),
            Amount = Convert.ToDecimal(reader["Amount"]),
            Method = reader["Method"]?.ToString() ?? string.Empty
        };
    }
}
