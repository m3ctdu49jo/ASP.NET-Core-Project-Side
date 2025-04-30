using ShoppingMall.DTOs;
using ShoppingMall.Models;

namespace ShoppingMall.Infrastructure.Services;

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
