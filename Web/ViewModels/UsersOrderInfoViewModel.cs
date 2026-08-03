using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.ViewModels
{
    public class UsersOrderInfoViewModel
    {
        public OrderDTO OrderDTO { get; set; }

        public IEnumerable<OrderDetailDTO>? OrderDetailDTOs { get; set; }
        public string? PaymentName { get; set; }
        public string? StatusName { get; set; }
    }
}