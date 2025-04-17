using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingMall.Controllers;
using ShoppingMall.DTOs;
using ShoppingMall.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingMall.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockCustomerService.Object);
        }

        /*[Fact]
        public async Task GetCustomers_ReturnsOkResult()
        {
            // Arrange
            var customers = new List<CustomerDTO>
            {
                new CustomerDTO { CustomerID = "ALFKI", CompanyName = "Alfreds Futterkiste" },
                new CustomerDTO { CustomerID = "ANATR", CompanyName = "Ana Trujillo Emparedados" }
            };
            _mockCustomerService.Setup(s => s.GetAllCustomersAsync())
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCustomer_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var customerId = "ALFKI";
            var customer = new CustomerDTO { CustomerID = customerId, CompanyName = "Alfreds Futterkiste" };
            _mockCustomerService.Setup(s => s.GetCustomerByIdAsync(customerId))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomer(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDTO>(okResult.Value);
            Assert.Equal(customerId, returnValue.CustomerID);
        }

        [Fact]
        public async Task GetCustomer_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var customerId = "INVALID";
            _mockCustomerService.Setup(s => s.GetCustomerByIdAsync(customerId))
                .ReturnsAsync((CustomerDTO)null);

            // Act
            var result = await _controller.GetCustomer(customerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCustomer_WithValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                CustomerID = "NEWID",
                CompanyName = "New Company",
                ContactName = "John Doe",
                ContactTitle = "Manager",
                Address = "123 Street",
                City = "City",
                Country = "Country",
                Phone = "123-456-7890"
            };

            _mockCustomerService.Setup(s => s.CreateCustomerAsync(customerDto))
                .ReturnsAsync(customerDto);

            // Act
            var result = await _controller.CreateCustomer(customerDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(CustomerController.GetCustomer), createdAtResult.ActionName);
            Assert.Equal(customerDto.CustomerID, createdAtResult.RouteValues["id"]);
        }

        [Fact]
        public async Task UpdateCustomer_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var customerId = "ALFKI";
            var customerDto = new CustomerDTO
            {
                CustomerID = customerId,
                CompanyName = "Updated Company"
            };

            _mockCustomerService.Setup(s => s.UpdateCustomerAsync(customerId, customerDto))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateCustomer(customerId, customerDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCustomer_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var customerId = "INVALID";
            var customerDto = new CustomerDTO { CustomerID = customerId };
            _mockCustomerService.Setup(s => s.UpdateCustomerAsync(customerId, customerDto))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateCustomer(customerId, customerDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var customerId = "ALFKI";
            _mockCustomerService.Setup(s => s.DeleteCustomerAsync(customerId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var customerId = "INVALID";
            _mockCustomerService.Setup(s => s.DeleteCustomerAsync(customerId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }*/
    }
} 