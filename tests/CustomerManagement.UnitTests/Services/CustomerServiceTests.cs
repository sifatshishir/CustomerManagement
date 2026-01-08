using Moq;
using CustomerManagement.Data;
using CustomerManagement.Data.Infrastructure;
using CustomerManagement.Models;
using Xunit;

namespace CustomerManagement.UnitTests.Services;

public class CustomerServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<ICustomerRepository> _mockCustomerRepo;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockCustomerRepo = new Mock<ICustomerRepository>();
        
        // Setup UoW to return the mock repository
        _mockUow.Setup(u => u.Customers).Returns(_mockCustomerRepo.Object);
        
        // Setup the factory to return the mock UoW
        Func<IUnitOfWork> uowFactory = () => _mockUow.Object;
        _service = new CustomerService(uowFactory);
    }

    [Fact]
    public void Add_ValidCustomer_CallsRepositoryAndCommits()
    {
        // Arrange
        var customer = new Customer { FirstName = "John", LastName = "Doe" };

        // Act
        _service.Add(customer);

        // Assert
        _mockCustomerRepo.Verify(r => r.Add(customer), Times.Once);
        _mockUow.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public void Add_NullCustomer_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.Add(null!));
    }

    [Fact]
    public void Add_RepositoryFails_RollsBackAndThrows()
    {
        // Arrange
        var customer = new Customer { FirstName = "Fail" };
        _mockCustomerRepo.Setup(r => r.Add(It.IsAny<Customer>())).Throws(new Exception("DB Error"));

        // Act & Assert
        var ex = Assert.Throws<ServiceException>(() => _service.Add(customer));
        Assert.Equal("Error adding customer", ex.Message);
        _mockUow.Verify(u => u.Rollback(), Times.Once);
        _mockUow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public void Import_MultipleCustomers_UsesTransactionAndCommitsOnce()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { FirstName = "C1" },
            new() { FirstName = "C2" }
        };

        // Act
        _service.Import(customers);

        // Assert
        _mockUow.Verify(u => u.BeginTransaction(), Times.Once);
        _mockCustomerRepo.Verify(r => r.Add(It.IsAny<Customer>()), Times.Exactly(2));
        _mockUow.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public void Import_AnyAdditionFails_RollsBackEverything()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { FirstName = "Success" },
            new() { FirstName = "Fail" }
        };

        _mockCustomerRepo.Setup(r => r.Add(It.Is<Customer>(c => c.FirstName == "Fail")))
            .Throws(new Exception("Import Error"));

        // Act & Assert
        Assert.Throws<ServiceException>(() => _service.Import(customers));
        _mockUow.Verify(u => u.Rollback(), Times.Once);
        _mockUow.Verify(u => u.Commit(), Times.Never);
    }
}
