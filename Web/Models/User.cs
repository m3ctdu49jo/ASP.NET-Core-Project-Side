namespace ShoppingMall.Web.Models;

public class User
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get;set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get;set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public string? Country { get;set; } = string.Empty;
    public string? City { get;set; } = string.Empty;
    public DateTime? CreatDate { get; set; }
    public DateTime? UpdateDate { get; set; } = DateTime.Now;
    public DateTime? LastLoginDate { get; set; }
    public DateTime? NewLoginDate { get; set; }
}
