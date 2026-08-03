using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Models;
using System;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Infrastructure.Repositories
{
    public class ShoppingCartUnitOfWork : IShoppingCartUnitOfWork
    {
        private readonly NorthwindContext _context;
        private IRepository<Order> _orders;
        private IRepository<OrderDetail> _orderDetails;
        private IRepository<ShoppingCart> _shoppingCart;
        private bool _disposed;

        public ShoppingCartUnitOfWork(NorthwindContext context)
        {
            _context = context;
        }
        public IRepository<ShoppingCart> ShoppingCarts =>
            _shoppingCart ??= new Repository<ShoppingCart>(_context);

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