using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web;

public class ShoppingCartRepository : Repository<ShoppingCart>, IRepository<ShoppingCart>, IShoppingCartRepository
{
    private readonly NorthwindContext _context;
    public ShoppingCartRepository(NorthwindContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShoppingCart>> GetAllIncludeProductByUserNameAsync(string userName)
    {
        return await _context.ShoppingCarts
            .Include(x => x.Product)
            .Where(x => x.UserName.Equals(userName))
            .ToListAsync();
    }
}
