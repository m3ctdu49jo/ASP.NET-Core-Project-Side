using System.ComponentModel.DataAnnotations;

namespace ShoppingMall.Web.DTOs
{
    public class CustomerDTO
    {
        [Required(ErrorMessage = "客戶編號為必填")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "客戶編號必須是5個字元")]
        [RegularExpression(@"^[A-Za-z0-9]{5}$", ErrorMessage = "客戶編號只能包含英文字母和數字")]
        [Display(Name = "客戶ID")]
        public string CustomerID { get; set; } = string.Empty;

        [Required(ErrorMessage = "公司名稱為必填")]
        [StringLength(40, ErrorMessage = "公司名稱不能超過40個字元")]
        [Display(Name = "公司名稱")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "聯絡人姓名為必填")]
        [StringLength(30, ErrorMessage = "聯絡人姓名不能超過30個字元")]
        [Display(Name = "聯絡人姓名")]
        public string ContactName { get; set; } = string.Empty;

        [StringLength(30, ErrorMessage = "聯絡人職稱不能超過30個字元")]
        [Display(Name = "聯絡人職稱")]
        public string ContactTitle { get; set; } = string.Empty;

        [StringLength(60, ErrorMessage = "地址不能超過60個字元")]
        [Display(Name = "地址")]
        public string Address { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "城市不能超過15個字元")]
        [Display(Name = "城市")]
        public string City { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "地區不能超過15個字元")]
        [Display(Name = "地區")]
        public string Region { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "郵遞區號不能超過10個字元")]
        [Display(Name = "郵遞區號")]
        public string PostalCode { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "國家不能超過15個字元")]
        [Display(Name = "國家")]
        public string Country { get; set; } = string.Empty;

        [RegularExpression(@"^[\d\-\(\)\s]*$", ErrorMessage = "電話號碼格式不正確")]
        [Display(Name = "電話")]
        public string Phone { get; set; } = string.Empty;

        [RegularExpression(@"^[\d\-\(\)\s]*$", ErrorMessage = "傳真號碼格式不正確")]
        [Display(Name = "傳真")]
        public string Fax { get; set; } = string.Empty;
    }
}