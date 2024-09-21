
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Tshop.Data;
using Tshop.Helpers;
using Tshop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<TshopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TShop"));
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// https://docs.automapper.org/en/stable/Dependency-injection.html
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-8.0
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/KhachHang/DangNhap";
    options.AccessDeniedPath = "/AccessDenied";
});



builder.Services.AddSingleton<IVnPayService, VnPayService>();


var app = builder.Build();
builder.Services.AddSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
	// Xác định route cho các Razor Pages trong Area
	endpoints.MapAreaControllerRoute(
		name: "Areas",
		areaName: "Admin", // Thay "YourAreaName" bằng tên của Area
		pattern: "{area:exists}/{controller=HomeAdmin}/{action=DangNhap}/{id?}");

	// Xác định route chính cho các Razor Pages
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");

	// Thêm route cho Razor Pages
	endpoints.MapRazorPages();
});
app.Run();

