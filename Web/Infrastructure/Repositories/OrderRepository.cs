using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly NorthwindContext _context;
    public OrderRepository(NorthwindContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Order>> GetListByUserId(Guid userId)
    {
        var items = await _context.Orders.Where(o => o.UserID.Equals(userId)).Include(p => p.OrderDetails).ToListAsync();
        return items;
    }
}
