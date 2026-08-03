using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.Controllers;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;
using ShoppingMall.Web.ViewModels;

namespace ShoppingMall.Web.Infrastructure.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IGenericService<ShoppingCart> _genericService;
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly NorthwindContext _dbContext;
    private readonly IMapper _mapper;
    public ShoppingCartService(
        IGenericService<ShoppingCart> genericService,
        IShoppingCartRepository shoppingCartRepository,
        NorthwindContext dbcontext,
        IMapper mapper)
    {
        _genericService = genericService;
        _shoppingCartRepository = shoppingCartRepository;
        _dbContext = dbcontext;
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

    public async Task Checkout(User user, string orderNum, ShippingInfo shippingInfo)
    {
        await CreateOrder(user.UserId, orderNum, shippingInfo);
        await CreateOrderDetail(user.UserName, orderNum);
        await clearShoppingCart(user.UserName);
        _dbContext.SaveChanges();
    }

    private async Task CreateOrder(Guid guid, string orderNum, ShippingInfo shippingInfo)
    {
        await _dbContext.Orders.AddAsync(
            new Order
            {
                OrderNum = orderNum,
                UserID = guid,
                OrderDate = DateTime.Now,
                ShipPhone = shippingInfo.Phone,
                ShipCity = shippingInfo.City,
                ShipAddress = shippingInfo.Address,
                ShipName = shippingInfo.ContactName,
                Payment = shippingInfo.Payment
            });
    }
    private async Task CreateOrderDetail(string userName, string orderNum)
    {
        var cartItem = await GetAllIncludeProductByUserNameAsync(userName);

        foreach (var item in cartItem)
        {
            await _dbContext.OrderDetails.AddAsync(
                new OrderDetail
                {
                    OrderNum = orderNum,
                    ProductID = item.Product.ProductID,
                    Quantity = (short)item.PurchCount,
                    UnitPrice = Convert.ToDecimal(item.Product.UnitPrice)
                }
            );
        }
    }
    private async Task clearShoppingCart(string userName)
    {
        var cartItem = await GetAllIncludeProductByUserNameAsync(userName);

        foreach (var item in cartItem)
        {
            var productID = item.Product.ProductID;

            var items = await _shoppingCartRepository.FindAsync(x => x.Product.ProductID == productID && x.UserName.Equals(userName));
            var i = items.SingleOrDefault();
            if (i != null)
            {
                await _shoppingCartRepository.RemoveAsync(i);
            }
        }
    }
}
