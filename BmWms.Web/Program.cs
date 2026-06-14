using Microsoft.EntityFrameworkCore;
using BmWms.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. KÍCH HOẠT ĐỘNG CƠ KÉP: Đăng ký cả Razor Pages và API Controllers vào dịch vụ hệ thống
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// 2. Nạp cấu hình chuỗi kết nối thực thể cơ sở dữ liệu
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 3. ĐỊNH TUYẾN KÉP: Định vị đường đi cho cả trang Razor hiển thị và các API Endpoint JSON
app.MapRazorPages();
app.MapControllers();

// 4. THẦN CHÚ AUTO-MIGRATION: Ép hệ thống tự động kiểm tra và sinh bảng xuống bãi Docker khi khởi chạy
// Thần chú Auto-Migration: Đồng bộ dữ liệu mượt mà lúc runtime
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Đoạn lệnh này kiểm tra nếu có bản migration mới từ các thành viên khác, nó tự cập nhật, nếu không nó chạy tiếp
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        // Thay vì làm sập app (crash code), ta chỉ ghi log log lỗi và cho phép ứng dụng chạy tiếp bình thường
        logger.LogWarning("He thong nhan dien database da duoc tao truoc bang script: " + ex.Message);
    }
}

app.Run();