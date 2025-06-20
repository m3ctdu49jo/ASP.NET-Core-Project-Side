using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Data
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
        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            // typeof(NorthwindContext).Assembly 表示從當前專案的組件中加載所有
            // 配置類別(EntityTypeConfiguration<T>)，如上方"OrderConfiguration"。
            // 這樣可以避免手動為每個實體類別添加配置。
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NorthwindContext).Assembly);

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


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.UserName });
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(20);
                entity.Property(e => e.City).HasMaxLength(20);
                entity.Property(e => e.CreatDate).HasColumnType("datetime");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

            });
            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => new { e.ProductID, e.UserName });
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.Product)
                    .WithMany(s => s.ShoppingCarts);
            });
        }
    }
} 

