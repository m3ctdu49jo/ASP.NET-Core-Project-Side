using ShoppingMall.Web.DTOs;

namespace ShoppingMall.Web;

public class ProductViewModel
{
    public ProductDTO Product { get; set; }
    public bool IsInCollection { get; set; }
}
