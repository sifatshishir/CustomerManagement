namespace CustomerManagement.Models;

using System;

public sealed class Invoice
{
    public int Id { get; set; }
    
    public int CustomerId { get; set; }
    
    public DateTime Date { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public string Status { get; set; } = string.Empty; // e.g., "Pending", "Paid", "Cancelled"
}
