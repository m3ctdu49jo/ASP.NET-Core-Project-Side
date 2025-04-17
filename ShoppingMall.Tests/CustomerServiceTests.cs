using Xunit;
using Moq;
using AutoMapper;
using ShoppingMall.Infrastructure.Services;
using ShoppingMall.Infrastructure.Repositories;
using ShoppingMall.Models;
using ShoppingMall.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingMall.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomerService _customerService;
        private readonly Mock<IRepository<Customer>> _mockCustomerRepository;

        public CustomerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockCustomerRepository = new Mock<IRepository<Customer>>();
            
            _mockUnitOfWork.Setup(uow => uow.Customers).Returns(_mockCustomerRepository.Object);
            _customerService = new CustomerService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ExistingId_ReturnsCustomerDTO()
        {
            // Arrange
            var customerId = "ALFKI";
            var customer = new Customer { CustomerID = "ALFKI" };
            var customerDto = new CustomerDTO { CustomerID = "ALFKI" };

            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(customer);
            _mockMapper.Setup(mapper => mapper.Map<CustomerDTO>(customer))
                .Returns(customerDto);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ALFKI", result.CustomerID);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { CustomerID = "ALFKI" },
                new Customer { CustomerID = "ANATR" }
            };
            var customerDtos = new List<CustomerDTO>
            {
                new CustomerDTO { CustomerID = "ALFKI" },
                new CustomerDTO { CustomerID = "ANATR" }
            };

            _mockCustomerRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(customers);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CustomerDTO>>(customers))
                .Returns(customerDtos);

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
} 