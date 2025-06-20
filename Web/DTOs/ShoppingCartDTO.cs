using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.DTOs;

public class ShoppingCartDTO
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int PurchCount { get; set; }

}
