using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        public string? ShipPostalCode { get; set; }

        [StringLength(15)]
        public string ShipCountry { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderID);
            builder.Property(o =>o.CustomerID).HasMaxLength(5);
            builder.Property(o => o.OrderDate).HasColumnType("datetime");
            builder.Property(o => o.ShippedDate).HasColumnType("datatime");
            builder.Property(o => o.ShipName).HasMaxLength(40);
            builder.Property(o => o.ShipAddress).HasMaxLength(60);
            builder.Property(o => o.ShipCity).HasMaxLength(15);
            builder.HasOne(o => o.Customer)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.CustomerID);
        }
    }
} 