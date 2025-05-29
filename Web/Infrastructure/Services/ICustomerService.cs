using ShoppingMall.Web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Infrastructure.Services
{
    public interface ICustomerService
    {
        Task<CustomerDTO> GetCustomerByIdAsync(string id);
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> CreateCustomerAsync(CustomerDTO customerDto);
        Task<CustomerDTO> UpdateCustomerAsync(CustomerDTO customerDto);
        Task DeleteCustomerAsync(int id);
    }
}