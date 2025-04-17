using Microsoft.EntityFrameworkCore;
using ShoppingMall.Models;

namespace ShoppingMall.Infrastructure.Data
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID);
                entity.Property(e => e.CustomerID).HasMaxLength(5);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(40);
                entity.Property(e => e.ContactName).HasMaxLength(30);
                entity.Property(e => e.ContactTitle).HasMaxLength(30);
                entity.Property(e => e.Address).HasMaxLength(60);
                entity.Property(e => e.City).HasMaxLength(15);
                entity.Property(e => e.Region).HasMaxLength(15);
                entity.Property(e => e.PostalCode).HasMaxLength(10);
                entity.Property(e => e.Country).HasMaxLength(15);
                entity.Property(e => e.Phone).HasMaxLength(24);
                entity.Property(e => e.Fax).HasMaxLength(24);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(40);
                entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID);
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.ShippedDate).HasColumnType("datetime");
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(e => e.CustomerID);

            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.ProductID });
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(e => e.OrderID);
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(e => e.ProductID);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierID);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(40);
                entity.Property(e => e.ContactName).HasMaxLength(30);
                entity.Property(e => e.ContactTitle).HasMaxLength(30);
                entity.Property(e => e.Address).HasMaxLength(60);
                entity.Property(e => e.City).HasMaxLength(15);
                entity.Property(e => e.Region).HasMaxLength(15);
                entity.Property(e => e.PostalCode).HasMaxLength(10);
                entity.Property(e => e.Country).HasMaxLength(15);
            });
        }
    }
} 