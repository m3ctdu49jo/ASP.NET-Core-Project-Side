using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingMall.Web.Infrastructure.Data;
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;
using ShoppingMall.Web.Mappings;
using AutoMapper;
using ShoppingMall.Web;
using ShoppingMall.Web.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShoppingMall.Web.Filters;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("fixed-per-ip", httpContext =>
    {
        string factoryKey = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: factoryKey,
            factory: partition => new FixedWindowRateLimiterOptions
            {
                // 針對 "每一個 IP" 獨立計算
                PermitLimit = 10,   // 每個視窗最多允許 10 個請求
                Window = TimeSpan.FromSeconds(10),  // 視窗時間為 10 秒
                QueueLimit = 2, // 超過限制時，最多排隊 2 個請求
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }
        );
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // 添加這行以支援即時編譯


builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 設定Session的過期時間
    options.Cookie.HttpOnly = true; // 設定Cookie為HttpOnly，防止JavaScript訪問
    options.Cookie.IsEssential = true; // 設定Cookie為必要，確保在GDPR下仍然可用
});

// 配置資料庫
builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnection")));

// 配置AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// 註冊UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 註冊Respoitories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

// 註冊Services
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IProductCollectionService, ProductCollectionService>();
// 添加記憶體快取
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
    });


builder.Services.AddScoped<LoginAuthenticatedRedirectFilter>();
builder.Services.AddScoped<AuthenticatedFilter>();

// JSON檔案
builder.Configuration.AddJsonFile(Path.Combine("DataFile/", "TaiwanCity.json"), optional: true, reloadOnChange: true);

var app = builder.Build();
// 程式部署在 IIS、Nginx、Azure App Service 或 K8s Ingress
// 通常會拿到 代理伺服器 (Proxy) 的 IP（例如 127.0.0.1 或 Load Balancer 的內部 IP）
// 導致所有使用者的 Partition Key 都一樣，結果所有人共用 10 次額度，馬上就會被鎖死
// 可以設定應用程式信任 Proxy 傳來的標頭
// app.UseForwardedHeaders(new ForwardedHeadersOptions
// {
//     ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | 
//                        Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
// });
app.UseRateLimiter();

// 初始化資料庫
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<NorthwindContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "初始化資料庫時發生錯誤。");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

// 在 app.UseRouting() 之後、app.UseEndpoints() 之前，加上 Session
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Users",
    pattern: "Users/Edit/{id?}/{username?}");

app.Run();
