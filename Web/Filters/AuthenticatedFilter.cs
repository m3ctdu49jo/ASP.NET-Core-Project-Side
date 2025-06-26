using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShoppingMall.Web.Filters;
public class AuthenticatedFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (user == null || !user.Identity.IsAuthenticated)
        {
            var isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            var isFetch = context.HttpContext.Request.Headers["Fetch-Request"] == "true";
            if (isAjax || isFetch)
            {
                context.Result = new UnauthorizedResult(); // 401
            }
            else
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // 不需實作
    }
}