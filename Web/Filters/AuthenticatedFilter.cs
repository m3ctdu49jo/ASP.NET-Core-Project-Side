using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShoppingMall.Web.Filters;
public class AuthenticatedFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // 不需實作
    }
}