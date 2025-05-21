using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingMall.Infrastructure.Data;
using ShoppingMall.Infrastructure.Repositories;
using ShoppingMall.Infrastructure.Services;
using ShoppingMall.Models;
using ShoppingMall.Mappings;
using AutoMapper;
using ShoppingMall;
using ShoppingMall.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


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

// 註冊Services
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
// 添加記憶體快取
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
    });

var app = builder.Build();

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

app.Run();
