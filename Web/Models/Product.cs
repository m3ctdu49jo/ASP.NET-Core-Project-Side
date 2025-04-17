using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingMall.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "商品名稱為必填")]
        [StringLength(40, ErrorMessage = "商品名稱不能超過40個字")]
        public string ProductName { get; set; } = string.Empty;

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20, ErrorMessage = "數量單位不能超過20個字")]
        public string? QuantityPerUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [StringLength(500, ErrorMessage = "商品描述不能超過500個字")]
        public string? Description { get; set; }

        [StringLength(200, ErrorMessage = "圖片URL不能超過200個字")]
        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; }

        [ForeignKey("SupplierID")]
        public virtual Supplier? Supplier { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
} 