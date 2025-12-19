using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Migrations;

namespace ShoppingMall.Web;

public class ProductCollectionService : IProductCollectionService
{
    private readonly IGenericService<ProductCollection> _genericService;

    public ProductCollectionService(IGenericService<ProductCollection> genericService)
    {
        _genericService = genericService;
    }
    public IGenericService<ProductCollection> Generic => _genericService;
}
