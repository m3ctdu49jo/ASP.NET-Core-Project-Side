using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingMall.Web.Models;

public class ShoppingCart
{
    [Key]
    public string UserName { get; set; }
    [Key]
    public int ProductID { get; set; }
    // virtual 讓 EF 支援延遲載入（Lazy Loading）
    [ForeignKey("ProductID")]
    public virtual Product Product { get; set; }
    public int PurchCount { get; set; }
}
