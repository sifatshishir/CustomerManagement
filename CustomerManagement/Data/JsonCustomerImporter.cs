namespace CustomerManagement.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using CustomerManagement.Models;

public sealed class JsonCustomerImporter
{
    private sealed class CustomerDto
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    public static List<Customer> ImportFromJson(string filePath)
    {
        var customers = new List<Customer>();

        try
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"File not found: {filePath}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return customers;
            }

            var json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var dtos = JsonSerializer.Deserialize<List<CustomerDto>>(json, options) ?? new List<CustomerDto>();

            var errors = new List<string>();

            foreach (var (dto, index) in dtos.Select((dto, idx) => (dto, idx + 1)))
            {
                var validationError = ValidateCustomer(dto);
                if (!string.IsNullOrEmpty(validationError))
                {
                    errors.Add($"Row {index}: {validationError}");
                    continue;
                }

                customers.Add(new Customer
                {
                    FirstName = dto.FirstName.Trim(),
                    LastName = dto.LastName.Trim(),
                    Age = dto.Age,
                    Type = dto.Type.Trim()
                });
            }

            if (errors.Count > 0)
            {
                var errorMessage = $"Successfully imported {customers.Count} customer(s), but {errors.Count} row(s) had validation errors:\n\n";
                errorMessage += string.Join("\n", errors);
                MessageBox.Show(errorMessage, "Import Completed with Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (customers.Count > 0)
            {
                MessageBox.Show($"Successfully imported {customers.Count} customer(s).", "Import Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (JsonException jsonEx)
        {
            MessageBox.Show($"Invalid JSON format: {jsonEx.Message}", "JSON Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error importing file: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return customers;
    }

    private static string ValidateCustomer(CustomerDto customer)
    {
        if (string.IsNullOrWhiteSpace(customer.FirstName))
            return "First name is required.";

        if (string.IsNullOrWhiteSpace(customer.LastName))
            return "Last name is required.";

        if (customer.Age <= 0 || customer.Age > 120)
            return $"Age must be between 1 and 120 (got {customer.Age}).";

        if (string.IsNullOrWhiteSpace(customer.Type))
            return "Type is required.";

        return string.Empty;
    }
}