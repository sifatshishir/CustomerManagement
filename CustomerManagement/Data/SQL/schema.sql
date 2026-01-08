-- Database Schema for Customer Management

CREATE DATABASE IF NOT EXISTS customermanagement;
USE customermanagement;

-- 1. Customers Table
CREATE TABLE IF NOT EXISTS Customers (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Type VARCHAR(50) NOT NULL,
    
    -- Optional fields mentioned in plan but not in current Model, adding for completeness if needed or sticking to Model
    -- Model has: Id, FirstName, LastName, Age, Type.
    -- Plan mentioned Phone, Email but Model file didn't have them. I will stick to Model to avoid runtime mismatch or add them as nullable.
    -- I'll stick to Model matches.
    
    INDEX idx_lastname (LastName)
);

-- 2. Invoices Table
CREATE TABLE IF NOT EXISTS Invoices (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CustomerId INT NOT NULL,
    Date DATETIME NOT NULL,
    TotalAmount DECIMAL(18, 2) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE,
    INDEX idx_customer_date (CustomerId, Date)
);

-- 3. Payments Table
CREATE TABLE IF NOT EXISTS Payments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    InvoiceId INT NOT NULL,
    Date DATETIME NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Method VARCHAR(50) NOT NULL,
    
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE,
    INDEX idx_invoice (InvoiceId)
);
