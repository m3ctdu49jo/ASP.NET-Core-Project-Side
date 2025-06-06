using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.ViewModels
{
    public class EditUserInfoViewModel
    {
        public UserDTO UserInfo { get; set; }

        public List<SelectListItem>? Cities { get; set; }
        public string? ResultMsg { get; set; }
    }
}