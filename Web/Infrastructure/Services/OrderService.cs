using System.Collections.ObjectModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;
using ShoppingMall.Web.ViewModels;

namespace ShoppingMall.Web.Infrastructure.Services;

public class OrderService : IOrderService
{
    NorthwindContext _context;
    IGenericService<Order> _genericService;
    IOrderRepository _orderRepo;
    IMapper _mapper;
    public OrderService(
        IGenericService<Order> genericService,
        IOrderRepository orderRepo,
        NorthwindContext context,
        IMapper mapper)
    {
        _context = context;
        _genericService = genericService;
        _orderRepo = orderRepo;
        _mapper = mapper;
    }

    public IGenericService<Order> Generic
    {
        get => _genericService;
    }

    public async Task<IEnumerable<OrderInfo>> GetOrdersByUserID(Guid userID)
    {

        var items = await _orderRepo.GetListByUserId(userID);
        IEnumerable<OrderInfo> orderList = new List<OrderInfo>();
        foreach (var item in items)
        {
            var i = new OrderInfo
            {
                orderDTO = _mapper.Map<OrderDTO>(item),
                TotalPrice = item.OrderDetails.Sum(m => m.Quantity * m.UnitPrice)
            };
            i.orderDTO.OrderDetails = item.OrderDetails;
            orderList = orderList.Append(i);
        }
        return orderList;
    }
    public async Task<Order> GetOrderByOrderNumAndUserID(string orderNum, Guid userID)
    {

        var items = await _context.Orders.Where(m => m.OrderNum == orderNum && m.UserID.Equals(userID)).Include(i => i.OrderDetails).FirstOrDefaultAsync();
        return items;
    }
}
