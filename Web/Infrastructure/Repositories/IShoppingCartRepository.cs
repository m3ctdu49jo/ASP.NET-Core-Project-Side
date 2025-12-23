using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Repositories;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    Task<IEnumerable<ShoppingCart>> GetAllIncludeProductByUserNameAsync(string userName);
}
