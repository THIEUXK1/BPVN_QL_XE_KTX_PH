using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(3); // Session tồn tại tối đa 3 giờ
    options.Cookie.HttpOnly = true; // Bảo mật hơn
    options.Cookie.IsEssential = true; // Cần thiết cho chức năng cơ bản
});

var app = builder.Build();

// Cấu hình lỗi cho môi trường Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Cấu hình middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Định tuyến cho các Area khác nhau
app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=DangNhap}/{action=Index}/{id?}");


// Định tuyến cho các controller mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
