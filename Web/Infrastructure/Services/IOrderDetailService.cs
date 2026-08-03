using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services
{
    public interface IOrderDetailService : IService<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetByOrderNum(string orderNum, Guid userID);
    }
}

