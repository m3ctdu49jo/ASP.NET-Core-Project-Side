using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public class OrderService : IOrderService
{
    IGenericService<Order, OrderDTO> _genericService;
    public OrderService(IGenericService<Order, OrderDTO> genericService)
    {
        _genericService = genericService;
    }

    public IGenericService<Order, OrderDTO> Generic
    {
        get => _genericService;
    }


}
