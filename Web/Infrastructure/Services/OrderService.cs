using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public class OrderService : IOrderService
{
    IGenericService<Order> _genericService;
    public OrderService(IGenericService<Order> genericService)
    {
        _genericService = genericService;
    }

    public IGenericService<Order> Generic
    {
        get => _genericService;
    }


}
