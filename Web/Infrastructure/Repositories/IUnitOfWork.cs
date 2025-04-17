using System;
using System.Threading.Tasks;
using ShoppingMall.Models;

namespace ShoppingMall.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Customer> Customers { get; }
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderDetail> OrderDetails { get; }
        Task<int> SaveChangesAsync();
    }
} 