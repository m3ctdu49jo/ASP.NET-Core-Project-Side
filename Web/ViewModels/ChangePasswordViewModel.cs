using System.ComponentModel;

namespace ShoppingMall.Web.ViewModels;

public class ChangePasswordViewModel
{
    [DisplayName("舊密碼")]
    public string OldPassword { get; set; } = string.Empty;
    [DisplayName("新密碼")]
    public string NewPassword { get; set; } = string.Empty;
    [DisplayName("確認新密碼")]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string ResultMessage { get; set; } = string.Empty;
    public bool IsSuccess { get; set; } = false;
}
