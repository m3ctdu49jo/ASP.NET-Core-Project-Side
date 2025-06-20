using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.ViewModels
{
    public class RegisterUserViewModel
    {
        public UserDTO UserInfo { get; set; }
        [DisplayName("確認密碼")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        public List<SelectListItem>? Cities { get; set; }
    }
}