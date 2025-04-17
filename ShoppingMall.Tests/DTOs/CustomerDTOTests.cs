using System.ComponentModel.DataAnnotations;
using ShoppingMall.DTOs;
using Xunit;

namespace ShoppingMall.Tests.DTOs
{
    public class CustomerDTOTests
    {
        /*[Fact]
        public void CustomerDTO_WithValidData_PassesValidation()
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
            var validationContext = new ValidationContext(customerDto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(customerDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "CustomerID 不能為空")]
        [InlineData("A", "CustomerID 長度必須在 5 到 5 個字符之間")]
        [InlineData("12345", "CustomerID 格式不正確")]
        public void CustomerDTO_WithInvalidCustomerID_FailsValidation(string customerId, string expectedError)
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                CustomerID = customerId,
                CompanyName = "Test Company"
            };

            // Act
            var validationContext = new ValidationContext(customerDto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(customerDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.ErrorMessage == expectedError);
        }

        [Theory]
        [InlineData("", "CompanyName 不能為空")]
        [InlineData("A", "CompanyName 長度必須在 2 到 40 個字符之間")]
        public void CustomerDTO_WithInvalidCompanyName_FailsValidation(string companyName, string expectedError)
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                CustomerID = "ALFKI",
                CompanyName = companyName
            };

            // Act
            var validationContext = new ValidationContext(customerDto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(customerDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.ErrorMessage == expectedError);
        }

        [Theory]
        [InlineData("123-456-7890", true)]
        [InlineData("(123) 456-7890", true)]
        [InlineData("123.456.7890", true)]
        [InlineData("1234567890", false)]
        [InlineData("123-456", false)]
        public void CustomerDTO_WithPhoneNumber_ValidatesCorrectly(string phone, bool shouldBeValid)
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                CustomerID = "ALFKI",
                CompanyName = "Test Company",
                Phone = phone
            };

            // Act
            var validationContext = new ValidationContext(customerDto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(customerDto, validationContext, validationResults, true);

            // Assert
            Assert.Equal(shouldBeValid, isValid);
            if (!shouldBeValid)
            {
                Assert.Contains(validationResults, r => r.ErrorMessage == "Phone 格式不正確");
            }
        }

        [Theory]
        [InlineData("12345", true)]
        [InlineData("12345-6789", true)]
        [InlineData("123", false)]
        [InlineData("12345678901", false)]
        public void CustomerDTO_WithPostalCode_ValidatesCorrectly(string postalCode, bool shouldBeValid)
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                CustomerID = "ALFKI",
                CompanyName = "Test Company",
                PostalCode = postalCode
            };

            // Act
            var validationContext = new ValidationContext(customerDto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(customerDto, validationContext, validationResults, true);

            // Assert
            Assert.Equal(shouldBeValid, isValid);
            if (!shouldBeValid)
            {
                Assert.Contains(validationResults, r => r.ErrorMessage == "PostalCode 格式不正確");
            }
        }*/
    }
} 