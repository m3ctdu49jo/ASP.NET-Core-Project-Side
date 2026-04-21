using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingMall.Web.Infrastructure.Data;

namespace ShoppingMall.Web.Filters;

/// <summary>
/// 交易失敗自動回滾
/// </summary>
public class TransactionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var dbContext = context.HttpContext.RequestServices.GetService<NorthwindContext>();
        dbContext.Database.BeginTransaction();
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var dbContext = context.HttpContext.RequestServices.GetService<NorthwindContext>();
        var transaction = dbContext.Database.CurrentTransaction;

        if(transaction == null) return;

        try
        {
            if(context.Exception == null)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }
        }
        finally
        {
            transaction.Dispose();
        }
    }
}