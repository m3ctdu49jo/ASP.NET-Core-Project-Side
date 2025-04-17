using AutoMapper;
using ShoppingMall.DTOs;
using ShoppingMall.Models;
using ShoppingMall.Infrastructure.Repositories;

namespace ShoppingMall.Infrastructure.Services
{
    
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> CreateCustomerAsync(CustomerDTO customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<CustomerDTO> UpdateCustomerAsync(CustomerDTO customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            await _unitOfWork.Customers.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _unitOfWork.Customers.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}