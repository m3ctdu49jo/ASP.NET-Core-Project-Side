using Microsoft.EntityFrameworkCore;
using ShoppingMall.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingMall.Web.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(NorthwindContext context)
        {
            context.Database.EnsureCreated();

            // 檢查是否已有資料
            if (context.Suppliers.Any())
            {
                return;   // 資料庫已經有資料
            }

            // 添加類別
            var categories = context.Categories.ToList();
            /*var categories = new Category[]
            {
                new Category { CategoryName = "飲料", Description = "軟性飲料、咖啡、茶、啤酒和淡啤酒" },
                new Category { CategoryName = "調味品", Description = "甜味和鹹味醬汁、調味品、醬料和調味品" },
                new Category { CategoryName = "甜點", Description = "甜點、糖果和甜麵包" },
                new Category { CategoryName = "乳製品", Description = "乳酪" },
                new Category { CategoryName = "穀物/穀類", Description = "麵包、穀物、義大利麵和麵食" },
                new Category { CategoryName = "肉類/家禽", Description = "預製肉類" },
                new Category { CategoryName = "農產品", Description = "乾果和豆類" },
                new Category { CategoryName = "海鮮", Description = "海鮮" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();*/

            // 添加供應商
            var suppliers = new Supplier[]
            {
                new Supplier { CompanyName = "供應商A", ContactName = "張三", ContactTitle = "銷售經理", Address = "台北市信義區", City = "台北", Region = "北部", PostalCode = "110", Country = "台灣", Phone = "02-1234-5678", Fax = "02-1234-5679" },
                new Supplier { CompanyName = "供應商B", ContactName = "李四", ContactTitle = "採購經理", Address = "高雄市前鎮區", City = "高雄", Region = "南部", PostalCode = "806", Country = "台灣", Phone = "07-1234-5678", Fax = "07-1234-5679" },
                new Supplier { CompanyName = "供應商C", ContactName = "王五", ContactTitle = "行銷經理", Address = "台中市西屯區", City = "台中", Region = "中部", PostalCode = "407", Country = "台灣", Phone = "04-1234-5678", Fax = "04-1234-5679" }
            };
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            // 添加商品
            var products = new Product[]
            {
                new Product {
                    ProductName = "可口可樂",
                    CategoryID = categories[0].CategoryID,
                    SupplierID = suppliers[0].SupplierID,
                    QuantityPerUnit = "24瓶/箱",
                    UnitPrice = 599.00M,
                    UnitsInStock = 100,
                    UnitsOnOrder = 0,
                    ReorderLevel = 20,
                    Discontinued = false,
                    Description = "經典可口可樂，清爽解渴",
                    ImageUrl = "https://example.com/cola.jpg",
                    CreatedDate = DateTime.Now
                },
                new Product {
                    ProductName = "黑胡椒醬",
                    CategoryID = categories[1].CategoryID,
                    SupplierID = suppliers[1].SupplierID,
                    QuantityPerUnit = "500g/瓶",
                    UnitPrice = 199.00M,
                    UnitsInStock = 50,
                    UnitsOnOrder = 0,
                    ReorderLevel = 10,
                    Discontinued = false,
                    Description = "香濃黑胡椒醬，適合牛排調味",
                    ImageUrl = "https://example.com/pepper.jpg",
                    CreatedDate = DateTime.Now
                },
                new Product {
                    ProductName = "巧克力蛋糕",
                    CategoryID = categories[2].CategoryID,
                    SupplierID = suppliers[2].SupplierID,
                    QuantityPerUnit = "1個/盒",
                    UnitPrice = 299.00M,
                    UnitsInStock = 30,
                    UnitsOnOrder = 0,
                    ReorderLevel = 5,
                    Discontinued = false,
                    Description = "濃郁巧克力蛋糕，口感綿密",
                    ImageUrl = "https://example.com/cake.jpg",
                    CreatedDate = DateTime.Now
                },
                new Product {
                    ProductName = "切達起司",
                    CategoryID = categories[3].CategoryID,
                    SupplierID = suppliers[0].SupplierID,
                    QuantityPerUnit = "200g/包",
                    UnitPrice = 159.00M,
                    UnitsInStock = 40,
                    UnitsOnOrder = 0,
                    ReorderLevel = 8,
                    Discontinued = false,
                    Description = "香濃切達起司，適合料理或直接食用",
                    ImageUrl = "https://example.com/cheese.jpg",
                    CreatedDate = DateTime.Now
                },
                new Product {
                    ProductName = "義大利麵",
                    CategoryID = categories[4].CategoryID,
                    SupplierID = suppliers[1].SupplierID,
                    QuantityPerUnit = "500g/包",
                    UnitPrice = 89.00M,
                    UnitsInStock = 60,
                    UnitsOnOrder = 0,
                    ReorderLevel = 12,
                    Discontinued = false,
                    Description = "義大利進口麵條，口感彈牙",
                    ImageUrl = "https://example.com/pasta.jpg",
                    CreatedDate = DateTime.Now
                }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
} 