using System;
using System.Threading.Tasks;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Repositories
{
    public interface IShoppingCartUnitOfWork : IDisposable
    {
        IRepository<ShoppingCart> ShoppingCarts { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderDetail> OrderDetails { get; }
        Task<int> SaveChangesAsync();
    }
}