using AutoMapper;
using ShoppingMall.DTOs;
using ShoppingMall.Mappings;
using ShoppingMall.Models;
using Xunit;

namespace ShoppingMall.Tests.Mappings
{
    public class AutoMapperProfileTests
    {
        private readonly IMapper _mapper;

        public AutoMapperProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();
        }

        /*[Fact]
        public void AutoMapperProfile_Configuration_IsValid()
        {
            // Arrange & Act & Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }*/

        [Fact]
        public void AutoMapperProfile_CustomerToCustomerDTO_MapsCorrectly()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerID = "ALFKI",
                CompanyName = "Alfreds Futterkiste",
                ContactName = "Maria Anders",
                ContactTitle = "Sales Representative",
                Address = "Obere Str. 57",
                City = "Berlin",
                Region = "Western Europe",
                PostalCode = "12209",
                Country = "Germany",
                Phone = "030-0074321",
                Fax = "030-0076545"
            };

            // Act
            var customerDto = _mapper.Map<CustomerDTO>(customer);

            // Assert
            Assert.NotNull(customerDto);
            Assert.Equal(customer.CustomerID, customerDto.CustomerID);
            Assert.Equal(customer.CompanyName, customerDto.CompanyName);
            Assert.Equal(customer.ContactName, customerDto.ContactName);
            Assert.Equal(customer.ContactTitle, customerDto.ContactTitle);
            Assert.Equal(customer.Address, customerDto.Address);
            Assert.Equal(customer.City, customerDto.City);
            Assert.Equal(customer.Region, customerDto.Region);
            Assert.Equal(customer.PostalCode, customerDto.PostalCode);
            Assert.Equal(customer.Country, customerDto.Country);
            Assert.Equal(customer.Phone, customerDto.Phone);
            Assert.Equal(customer.Fax, customerDto.Fax);
        }

        [Fact]
        public void AutoMapperProfile_CustomerDTOToCustomer_MapsCorrectly()
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                CustomerID = "ALFKI",
                CompanyName = "Alfreds Futterkiste",
                ContactName = "Maria Anders",
                ContactTitle = "Sales Representative",
                Address = "Obere Str. 57",
                City = "Berlin",
                Region = "Western Europe",
                PostalCode = "12209",
                Country = "Germany",
                Phone = "030-0074321",
                Fax = "030-0076545"
            };

            // Act
            var customer = _mapper.Map<Customer>(customerDto);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customerDto.CustomerID, customer.CustomerID);
            Assert.Equal(customerDto.CompanyName, customer.CompanyName);
            Assert.Equal(customerDto.ContactName, customer.ContactName);
            Assert.Equal(customerDto.ContactTitle, customer.ContactTitle);
            Assert.Equal(customerDto.Address, customer.Address);
            Assert.Equal(customerDto.City, customer.City);
            Assert.Equal(customerDto.Region, customer.Region);
            Assert.Equal(customerDto.PostalCode, customer.PostalCode);
            Assert.Equal(customerDto.Country, customer.Country);
            Assert.Equal(customerDto.Phone, customer.Phone);
            Assert.Equal(customerDto.Fax, customer.Fax);
        }
    }
} 