using System.ComponentModel.DataAnnotations;

namespace ShoppingMall.Web.Models.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "無")]
        None = 0,
        [Display(Name = "已確認")]
        Confirmed = 1,
        [Display(Name = "運送中")]
        Shipped = 2,
        [Display(Name = "已完成")]
        Completed = 3,
        [Display(Name = "已取消")]
        Cancelled = 4,
        [Display(Name = "已退貨")]
        Returned = 5
    }
}