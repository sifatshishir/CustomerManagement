namespace CustomerManagement.Data;

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using CustomerManagement.Models;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();
    private readonly string _dataFilePath;
    private int _nextId = 1;

    public InMemoryCustomerRepository(string? dataFilePath = null)
    {
        _dataFilePath = dataFilePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "customers.json");
        LoadFromJson();
    }

    public IEnumerable<Customer> GetAll()
    {
        // Return a copy to mimic DB behavior (snapshot)
        return _customers.ToList();
    }

    public void Add(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        
        // Auto-increment ID simulation
        if (customer.Id <= 0)
        {
            customer.Id = _nextId++;
        }
        else if (customer.Id >= _nextId)
        {
            _nextId = customer.Id + 1;
        }

        _customers.Add(customer);
    }

    public void Update(Customer customer) 
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        
        var index = _customers.FindIndex(c => c.Id == customer.Id);
        if (index != -1)
        {
            _customers[index] = customer;
        }
    }

    public void Delete(int id)
    {
        var existing = _customers.FirstOrDefault(c => c.Id == id);
        if (existing != null) _customers.Remove(existing);
    }

    public Customer? FindById(int id) => _customers.FirstOrDefault(c => c.Id == id);

    public void SaveToJson()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(_customers, options);
            File.WriteAllText(_dataFilePath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error saving customers to JSON file: {ex.Message}", ex);
        }
    }

    public void Reload()
    {
        LoadFromJson();
    }

    private void LoadFromJson()
    {
        _customers.Clear(); // Clear existing
        try
        {
            if (!File.Exists(_dataFilePath))
            {
                File.WriteAllText(_dataFilePath, "[]");
                return;
            }

            var json = File.ReadAllText(_dataFilePath);
            if (string.IsNullOrWhiteSpace(json)) return;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var customers = JsonSerializer.Deserialize<List<Customer>>(json, options) ?? new List<Customer>();

            _customers.AddRange(customers);
            if (_customers.Any())
            {
                _nextId = _customers.Max(c => c.Id) + 1;
            }
        }
        catch (Exception)
        {
            // Fallback for corrupt file
            _customers.Clear();
        }
    }
}