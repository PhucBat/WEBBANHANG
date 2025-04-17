using Microsoft.AspNetCore.Identity;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebsiteBanHang.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".WebsiteBanHang.Session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Cấu hình Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
// Đăng ký ProductRepository
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, EFOrderDetailRepository>();
builder.Services.AddScoped<ShoppingCartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Cấu hình routing cho admin area
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Cấu hình routing mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

// Cấu hình cho phép truy cập giỏ hàng mà không cần đăng nhập
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "cart",
        pattern: "ShoppingCart/{action=Index}/{id?}",
        defaults: new { controller = "ShoppingCart" });
    
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Product}/{action=Index}/{id?}");
    
    endpoints.MapRazorPages();
});

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

app.Run();

