using System.ComponentModel.DataAnnotations;

namespace ShoppingMall.Web;

public class ForgotPasswordViewModel
{
    public string UserName { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ResultMsg { get; set; } = string.Empty;
}
