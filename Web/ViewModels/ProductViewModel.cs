using ShoppingMall.Web.DTOs;

namespace ShoppingMall.Web.ViewModels;

public class ProductViewModel
{
    public ProductDTO Product { get; set; }
    public bool IsInCollection { get; set; }
}
