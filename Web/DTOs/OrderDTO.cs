using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.DTOs
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public string OrderNum { get; set; }
        public string CustomerID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string? ShipPhone { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public string CustomerName { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}