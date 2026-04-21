using System.Net;
using System.Text.Json;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace ShoppingMall.Web.Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "發生未處理的例外狀況: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "appliction/json";

        // 根據例外型別決定 Status Code，區分商業邏輯錯誤與系統錯誤)
        context.Response.StatusCode = exception switch
        {
          ApplicationException => (int)HttpStatusCode.BadRequest,  
          KeyNotFoundException => (int)HttpStatusCode.NotFound,
          _ => (int)HttpStatusCode.InternalServerError
        };

        // 開發環境給詳細 StackTrace，正式環境只給模糊訊息
        var response = new
        {
          StatusCode = context.Response.StatusCode,
          Message = _env.IsDevelopment() ? exception.Message : "伺服器發生錯誤，請稍後再試",
          Detail = _env.IsDevelopment() ? exception.StackTrace : null
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}