using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.ViewModels
{
    public class ShoppingCarOrderViewModel
    {
        public ShippingInfo Shipping { get; set; } = new();
    }
}