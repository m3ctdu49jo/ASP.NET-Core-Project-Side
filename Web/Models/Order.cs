using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingMall.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [StringLength(5)]
        public string CustomerID { get; set; }

        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Freight { get; set; }

        [StringLength(40)]
        public string ShipName { get; set; }

        [StringLength(60)]
        public string ShipAddress { get; set; }

        [StringLength(15)]
        public string ShipCity { get; set; }

        [StringLength(15)]
        public string ShipRegion { get; set; }

        [StringLength(10)]
        public string ShipPostalCode { get; set; }

        [StringLength(15)]
        public string ShipCountry { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
} 