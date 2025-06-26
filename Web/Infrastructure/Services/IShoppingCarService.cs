using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public interface IShoppingCartService : IService<ShoppingCart>
{
    Task<ShoppingCart> GetByIdAndUserNameAsync(int productId, string userName);
    Task<IEnumerable<ShoppingCartDTO>> GetAllByUserNameAsync(string userName);
    Task<IEnumerable<ShoppingCartDTO>> GetAllIncludeProductByUserNameAsync(string userName);
    Task<bool> DeleteAsync(int productId, string userName);
}
