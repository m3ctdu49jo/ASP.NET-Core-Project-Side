using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public interface IProductCollectionService : IService<ProductCollection>
{
    public Task<IEnumerable<ProductCollection>> GetListByUserName(string userName);
}
