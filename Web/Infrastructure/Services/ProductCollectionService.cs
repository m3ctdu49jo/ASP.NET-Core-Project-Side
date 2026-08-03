
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public class ProductCollectionService : IProductCollectionService
{
    private readonly IGenericService<ProductCollection> _genericService;
    private readonly IProductCollectionRepository _productCollectionRepository;
    public ProductCollectionService(
        IGenericService<ProductCollection> genericService,
        IProductCollectionRepository productCollectionRepository
    )
    {
        _genericService = genericService;
        _productCollectionRepository = productCollectionRepository;
    }

    public IGenericService<ProductCollection> Generic => _genericService;

    public async Task<IEnumerable<ProductCollection>> GetListByUserName(string userName)
    {
        return await _productCollectionRepository.GetListByUserName(userName);
    }
}
