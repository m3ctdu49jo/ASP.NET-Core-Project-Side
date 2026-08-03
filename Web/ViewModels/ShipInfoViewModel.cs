using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.ViewModels
{
    public class ShipInfoViewModel
    {
        public List<ShoppingCartDTO>? CartItems { get; set; }
        public List<SelectListItem>? Cities { get; set; }
        public ShippingInfo Shipping { get; set; } = new();
        public bool Ship_equal_user { get; set; } = false;
        public string? ResultMsg { get; set; }

    }
}