using System.ComponentModel.DataAnnotations;

namespace ShoppingMall.Web.Models.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "無")]
        None = 0,
        [Display(Name = "ATM 匯款")]
        ATM = 1,
        [Display(Name = "信用卡")]
        CreditCard = 2,
        [Display(Name = "Line Pay")]
        LinePay = 3
    }
}