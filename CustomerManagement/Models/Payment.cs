namespace CustomerManagement.Models;

using System;

public sealed class Payment
{
    public int Id { get; set; }
    
    public int InvoiceId { get; set; }
    
    public DateTime Date { get; set; }
    
    public decimal Amount { get; set; }
    
    public string Method { get; set; } = string.Empty; // e.g., "Credit Card", "Bank Transfer", "Cash"
}
