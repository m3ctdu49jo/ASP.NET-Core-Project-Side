using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetListByUserId(Guid userId);
}
