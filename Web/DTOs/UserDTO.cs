using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingMall.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    [DisplayName("帳號")]
    public string UserName { get; set; } = string.Empty;
    [DisplayName("密碼")]
    public string Password { get;set; } = string.Empty;
    [DisplayName("姓名")]
    public string Name { get; set; } = string.Empty;
    [DisplayName("E-mail")]
    public string? Email { get;set; } = string.Empty;
    [DisplayName("手機號碼")]
    public string? Phone { get; set; } = string.Empty;
    [DisplayName("地址")]
    public string? Address { get; set; } = string.Empty;
    [DisplayName("國家")]
    public string? Country { get;set; } = string.Empty;
    [DisplayName("城市")]
    public string? City { get;set; } = string.Empty;
    public DateTime? CreatDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
}
