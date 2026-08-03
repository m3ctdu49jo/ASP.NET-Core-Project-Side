using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.ViewModels
{
    public class OrderInfo
    {
        public OrderDTO orderDTO { get; set; }
        public decimal TotalPrice { get; set; }


    }
}