using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingMall.Controllers;
using System.Security.Claims;

namespace ShoppingMall.Web.Filters
{
    public class LoginAuthenticatedRedirectFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            var actionName = (context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)?.ActionName;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
            else if (context.Controller is LoginController && string.Equals(actionName, "Index", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            else if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
            else
            {
                // 如果是 LoginController && action 是 Index，則不進行重定向
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 不需實作
        }
    }
}
