using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Repositories;

public interface IProductCollectionRepository
{
    public Task<IEnumerable<ProductCollection>> GetListByUserName(string userName);
}
