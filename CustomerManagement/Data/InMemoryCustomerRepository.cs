namespace CustomerManagement.Data;

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CustomerManagement.Models;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly BindingList<Customer> _customers = new();
    private readonly string _dataFilePath;
    private int _nextId = 1;

    public BindingList<Customer> Customers => _customers;

    public InMemoryCustomerRepository(string? dataFilePath = null)
    {
        _dataFilePath = dataFilePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "customers.json");
        LoadFromJson();
    }

    public void Add(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        customer.Id = _nextId++;
        _customers.Add(customer);
    }

    public void Update(Customer customer) 
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        var existing = FindById(customer.Id);
        if (existing is null) return;

        existing.FirstName = customer.FirstName;
        existing.LastName = customer.LastName;
        existing.Age = customer.Age;
        existing.Type = customer.Type;
    }

    public void Delete(int id)
    {
        var existing = FindById(id);
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

            var json = JsonSerializer.Serialize(_customers.ToList(), options);
            File.WriteAllText(_dataFilePath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error saving customers to JSON file: {ex.Message}", ex);
        }
    }

    private void LoadFromJson()
    {
        try
        {
            if (!File.Exists(_dataFilePath))
            {
                File.WriteAllText(_dataFilePath, "[]");
                return;
            }

            var json = File.ReadAllText(_dataFilePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var customers = JsonSerializer.Deserialize<List<CustomerDto>>(json, options) ?? new List<CustomerDto>();

            foreach (var dto in customers)
            {
                if (ValidateCustomerDto(dto))
                {
                    _customers.Add(new Customer
                    {
                        Id = dto.Id,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Age = dto.Age,
                        Type = dto.Type
                    });

                    // Track the highest ID to ensure new customers get unique IDs
                    if (dto.Id >= _nextId)
                    {
                        _nextId = dto.Id + 1;
                    }
                }
            }
        }
        catch (JsonException)
        {
            _customers.Clear();
        }
        catch (Exception)
        {
            _customers.Clear();
        }
    }

    private bool ValidateCustomerDto(CustomerDto customer)
    {
        return customer.Id > 0 &&
               !string.IsNullOrWhiteSpace(customer.FirstName) &&
               !string.IsNullOrWhiteSpace(customer.LastName) &&
               customer.Age > 0 && customer.Age <= 120 &&
               !string.IsNullOrWhiteSpace(customer.Type);
    }

    private sealed class CustomerDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}