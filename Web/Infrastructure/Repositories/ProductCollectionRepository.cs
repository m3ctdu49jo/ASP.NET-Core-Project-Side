using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Repositories;

public class ProductCollectionRepository : IProductCollectionRepository
{
    private readonly NorthwindContext _context;
    public ProductCollectionRepository(NorthwindContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ProductCollection>> GetListByUserName(string userName)
    {
        var items = await _context.ProductCollections.Where(pc => pc.UserName == userName).Include(p => p.Product).ToListAsync();
        return items;
    }
}
