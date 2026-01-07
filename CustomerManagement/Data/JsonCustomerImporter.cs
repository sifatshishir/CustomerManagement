namespace CustomerManagement.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using CustomerManagement.Models;

public sealed class JsonCustomerImporter
{
    public sealed class ImportResult
    {
        public bool Success { get; set; }
        public List<Customer> Customers { get; set; } = new();
        public List<string> ValidationErrors { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public string? WarningMessage { get; set; }
    }

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

    public static ImportResult ImportFromJson(string filePath)
    {
        var result = new ImportResult();

        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                result.ErrorMessage = "File path cannot be empty.";
                return result;
            }

            if (!File.Exists(filePath))
            {
                result.ErrorMessage = $"File not found: {filePath}";
                return result;
            }

            string json;
            try
            {
                json = File.ReadAllText(filePath);
            }
            catch (UnauthorizedAccessException ex)
            {
                result.ErrorMessage = $"Access denied to file: {ex.Message}";
                return result;
            }
            catch (IOException ex)
            {
                result.ErrorMessage = $"IO error reading file: {ex.Message}";
                return result;
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                result.ErrorMessage = "File is empty or contains no data.";
                return result;
            }

            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };

            List<CustomerDto>? dtos;
            try
            {
                dtos = JsonSerializer.Deserialize<List<CustomerDto>>(json, options);
            }
            catch (JsonException jsonEx)
            {
                result.ErrorMessage = $"Invalid JSON format at line {jsonEx.LineNumber}, position {jsonEx.BytePositionInLine}: {jsonEx.Message}";
                return result;
            }

            if (dtos == null || dtos.Count == 0)
            {
                result.ErrorMessage = "No customer data found in file.";
                return result;
            }

            var errors = new List<string>();

            foreach (var (dto, index) in dtos.Select((dto, idx) => (dto, idx + 1)))
            {
                if (dto == null)
                {
                    errors.Add($"Row {index}: Null customer entry.");
                    continue;
                }

                var validationError = ValidateCustomer(dto);
                if (!string.IsNullOrEmpty(validationError))
                {
                    errors.Add($"Row {index}: {validationError}");
                    continue;
                }

                try
                {
                    result.Customers.Add(new Customer
                    {
                        FirstName = dto.FirstName?.Trim() ?? string.Empty,
                        LastName = dto.LastName?.Trim() ?? string.Empty,
                        Age = dto.Age,
                        Type = dto.Type?.Trim() ?? string.Empty
                    });
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {index}: Error creating customer - {ex.Message}");
                }
            }

            result.ValidationErrors = errors;
            result.Success = true;

            if (errors.Count > 0 && result.Customers.Count > 0)
            {
                result.WarningMessage = $"Successfully imported {result.Customers.Count} customer(s), but {errors.Count} row(s) had validation errors.";
            }
            else if (errors.Count > 0 && result.Customers.Count == 0)
            {
                result.ErrorMessage = $"No valid customers found. {errors.Count} row(s) had validation errors.";
                result.Success = false;
            }
        }
        catch (JsonException jsonEx)
        {
            result.ErrorMessage = $"JSON parsing error: {jsonEx.Message}";
        }
        catch (OutOfMemoryException)
        {
            result.ErrorMessage = "File is too large to process. Please use a smaller file.";
        }
        catch (Exception ex)
        {
            result.ErrorMessage = $"Unexpected error importing file: {ex.Message}";
        }

        return result;
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