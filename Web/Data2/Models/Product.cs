using System;
using System.Collections.Generic;

namespace ShoppingMall.Web.Data2.Models;

public partial class Product
{
    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public int? CategoryID { get; set; }

    public string? QuantityPerUnit { get; set; }

    public decimal? UnitPrice { get; set; }

    public short? UnitsInStock { get; set; }

    public short? UnitsOnOrder { get; set; }

    public short? ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? SupplierID { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
