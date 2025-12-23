using System.ComponentModel.DataAnnotations.Schema;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Models;

public class ProductCollection
{
    public int ProductID { get; set; }
    public string UserName { get; set; }
    [ForeignKey("ProductID")]
    public Product Product { get; set; }
    public User User { get; set; }
}
