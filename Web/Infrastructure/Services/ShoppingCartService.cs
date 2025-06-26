using AutoMapper;
using ShoppingMall.Web.Controllers;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IGenericService<ShoppingCart> _genericService;
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IMapper _mapper;
    public ShoppingCartService(IGenericService<ShoppingCart> genericService, IShoppingCartRepository shoppingCartRepository, IMapper mapper)
    {
        _genericService = genericService;
        _shoppingCartRepository = shoppingCartRepository;
        _mapper = mapper;
    }
    public IGenericService<ShoppingCart> Generic => _genericService;

    public async Task<bool> DeleteAsync(int productId, string userName)
    {
        bool success = false;
        var items = await _shoppingCartRepository.FindAsync(x => x.Product.ProductID == productId && x.UserName.Equals(userName));
        var item = items.SingleOrDefault();
        if (item != null)
        {
            await _shoppingCartRepository.RemoveAsync(item);
            await _shoppingCartRepository.SaveChangesAsync();
            success = true;
        }
        return success;
    }

    public async Task<IEnumerable<ShoppingCartDTO>> GetAllByUserNameAsync(string userName)
    {
        var items = await _shoppingCartRepository.FindAsync(x => x.UserName.Equals(userName));
        return _mapper.Map<IEnumerable<ShoppingCartDTO>>(items);
    }

    public async Task<IEnumerable<ShoppingCartDTO>> GetAllIncludeProductByUserNameAsync(string userName)
    {
        var items = await _shoppingCartRepository.GetAllIncludeProductByUserNameAsync(userName);
        return _mapper.Map<IEnumerable<ShoppingCartDTO>>(items);
    }

    public async Task<ShoppingCart> GetByIdAndUserNameAsync(int productId, string userName)
    {
        var item = await _shoppingCartRepository.FindAsync(x => x.Product.ProductID == productId && x.UserName.Equals(userName));
        return item.FirstOrDefault();
    }
}
