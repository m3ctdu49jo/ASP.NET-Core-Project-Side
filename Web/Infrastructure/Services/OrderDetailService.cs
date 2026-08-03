using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public class OrderDetailService : IOrderDetailService
{
    IGenericService<OrderDetail> _genericService;
    NorthwindContext _context;
    public OrderDetailService(IGenericService<OrderDetail> genericService, NorthwindContext context)
    {
        _genericService = genericService;
        _context = context;
    }

    public IGenericService<OrderDetail> Generic
    {
        get => _genericService;
    }

    public async Task<IEnumerable<OrderDetail>> GetByOrderNum(string orderNum, Guid userID)
    {
        var item = await _context.OrderDetails.Where(m => m.OrderNum == orderNum && m.Order.UserID.Equals(userID)).Include(p => p.Product).Include(p => p.Order).ToListAsync();
        return item;
    }


}
