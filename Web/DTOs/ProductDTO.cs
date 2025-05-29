using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingMall.Web.DTOs
{
    public class ProductDTO
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "商品名稱為必填")]
        [StringLength(40, ErrorMessage = "商品名稱不能超過40個字")]
        public string ProductName { get; set; } = string.Empty;

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20, ErrorMessage = "數量單位不能超過20個字")]
        public string? QuantityPerUnit { get; set; }

        [Range(0, 999999.99, ErrorMessage = "單價必須在0到999999.99之間")]
        public decimal? UnitPrice { get; set; }

        [Range(0, 32767, ErrorMessage = "庫存數量必須在0到32767之間")]
        public short? UnitsInStock { get; set; }

        [Range(0, 32767, ErrorMessage = "訂購數量必須在0到32767之間")]
        public short? UnitsOnOrder { get; set; }

        [Range(0, 32767, ErrorMessage = "重新訂購點必須在0到32767之間")]
        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [StringLength(500, ErrorMessage = "商品描述不能超過500個字")]
        public string? Description { get; set; }

        [StringLength(200, ErrorMessage = "圖片URL不能超過200個字")]
        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // 導航屬性
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }
    }
} 