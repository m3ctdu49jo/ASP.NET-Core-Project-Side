using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    Task<IEnumerable<ShoppingCart>> GetAllIncludeProductByUserNameAsync(string userName);
}
