using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.DTOs
{
    public class OrderDetailDTO
    {

        public int OrderID { get; set; }
        public string OrderNum { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public string ProductName { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}