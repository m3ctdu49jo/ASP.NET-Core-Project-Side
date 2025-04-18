using ShoppingMall.DTOs;

namespace ShoppingMall.Infrastructure.Services;

public class OrderService : IOrderService
{
    IGenericService<OrderDTO> _genericService;
    public OrderService(IGenericService<OrderDTO> genericService)
    {
        _genericService = genericService;
    }

    public IGenericService<OrderDTO> Generic
    {
        get => _genericService;
    }
}
