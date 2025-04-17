using System.ComponentModel.DataAnnotations;

namespace ShoppingMall;

public class SupplierDTO
{
    public int SupplierID { get; set; }
    [Required(ErrorMessage = "公司名稱為必填")]
    public string CompanyName { get; set; }
    [Required(ErrorMessage = "聯絡人姓名為必填")]
    public string ContactName { get; set; }
    [Required(ErrorMessage = "地址為必填")]
    public string Address { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    [RegularExpression(@"^[0-9]{2,3}\d{7}$", ErrorMessage = "電話號碼格式不正確")]
    public string Phone { get; set; }
    [RegularExpression(@"^[0-9]{2,3}\d{7}$", ErrorMessage = "傳真號碼格式不正確")]
    public string Fax { get; set; }
    public string HomePage { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
