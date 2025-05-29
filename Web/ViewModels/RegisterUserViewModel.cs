using System.ComponentModel;
using ShoppingMall.Web.DTOs;

namespace ShoppingMall.Web.ViewModels
{
    public class RegisterUserViewModel
    {
        public UserDTO UserInfo { get; set; }
        [DisplayName("確認密碼")]
        public string ConfirmPassword { get;set; } = string.Empty;
    }
}