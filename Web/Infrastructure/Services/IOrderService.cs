using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;
using ShoppingMall.Web.ViewModels;

namespace ShoppingMall.Web.Infrastructure.Services
{
    public interface IOrderService : IService<Order>
    {
        Task<IEnumerable<OrderInfo>> GetOrdersByUserID(Guid userID);
        Task<Order> GetOrderByOrderNumAndUserID(string orderNum, Guid userID);
    }
}

