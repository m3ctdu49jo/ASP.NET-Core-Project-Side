using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.ViewModels
{
    // 將相同的ViewModel欄位驗證抽離
    public class ShippingInfo
    {
        [Required(ErrorMessage = "聯絡人姓名為必填")]
        [StringLength(30, ErrorMessage = "聯絡人姓名不能超過30個字元")]
        [Display(Name = "聯絡人姓名")]
        public string ContactName { get; set; } = string.Empty;

        [Required(ErrorMessage = "電話為必填")]
        [RegularExpression(@"^[\d\-\(\)\s]*$", ErrorMessage = "電話號碼格式不正確")]
        [Display(Name = "電話")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "地址為必填")]
        [StringLength(60, ErrorMessage = "地址不能超過60個字元")]
        [Display(Name = "地址")]
        public string Address { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "城市不能超過15個字元")]
        [Display(Name = "城市")]
        public string City { get; set; } = string.Empty;
        [RegularExpression(@"^[1-3]{1}", ErrorMessage = "請選擇正確的付款方式")]
        public short? Payment { get; set; }
    }
}