using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Models;
using System;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NorthwindContext _context;
        private IRepository<Customer> _customers;
        private IRepository<Product> _products;
        private IRepository<Category> _categories;
        private IRepository<Order> _orders;
        private IRepository<OrderDetail> _orderDetails;
        private bool _disposed;

        public UnitOfWork(NorthwindContext context)
        {
            _context = context;
        }

        public IRepository<Customer> Customers => 
            _customers ??= new Repository<Customer>(_context);
        public IRepository<Product> Products => 
            _products ??= new Repository<Product>(_context);

        public IRepository<Category> Categories => 
            _categories ??= new Repository<Category>(_context);

        public IRepository<Order> Orders => 
            _orders ??= new Repository<Order>(_context);

        public IRepository<OrderDetail> OrderDetails => 
            _orderDetails ??= new Repository<OrderDetail>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _disposed = true;
            }
        }
    }
} 