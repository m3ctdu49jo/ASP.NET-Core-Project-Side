using Xunit;
using Moq;
using AutoMapper;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;
using ShoppingMall.Web.DTOs;
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
            // 使用 Mock（模擬物件）可以隔離測試目標，確保測試只關注 CustomerService 的邏輯。
            // 在 CustomerService 中，IUnitOfWork 是一個依賴項，它提供了對 IRepository<Customer> 的訪問。
            // 因為 IUnitOfWork 是一個抽象介面，您需要模擬它的行為，讓它返回一個模擬的 IRepository<Customer>
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

            // 模擬 IRepository<Customer> 的 GetByIdAsync 方法。
            // 當測試程式呼叫 GetByIdAsync(customerId) 時，模擬物件會返回一個假資料（customer）。
            // ReturnsAsync(customer) 表示這是一個異步方法，並返回 customer 作為結果。
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

        public async Task GetCustomerByIdAsync_ExistingId_ReturnCustomerDTO2()
        {
            // Arrange
            var customerId= "ALFKI";
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

        public async Task GetAllCystomerAsync_ReturnsAllCustomers2()
        {
            // Arrange
            var customers = new List<Customer>{
                new Customer {CustomerID = "ALFKI"},
                new Customer {CustomerID = "ANATR"}
            };
            var customerDtos = new List<CustomerDTO>{
                new CustomerDTO {CustomerID = "ALFKI"},
                new CustomerDTO {CustomerID = "ANATR"}
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