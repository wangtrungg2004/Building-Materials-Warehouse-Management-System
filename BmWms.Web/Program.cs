using Microsoft.EntityFrameworkCore;
using BmWms.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using BmWms.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. KÍCH HOẠT ĐỘNG CƠ KÉP: Đăng ký cả Razor Pages và API Controllers vào dịch vụ hệ thống
builder.Services.AddRazorPages();
builder.Services.AddControllers();
// ── Auth Services ─────────────────────────────────────────────────────────
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IOtpService, OtpService>();

// ── Product & ProductGroup Services ───────────────────────────────────────
builder.Services.AddScoped<BmWms.Infrastructure.Repositories.Interfaces.IProductGroupRepository, BmWms.Infrastructure.Repositories.Implementations.ProductGroupRepository>();
builder.Services.AddScoped<BmWms.Infrastructure.Repositories.Interfaces.IProductRepository, BmWms.Infrastructure.Repositories.Implementations.ProductRepository>();
builder.Services.AddScoped<BmWms.Business.Services.IProductGroupService, BmWms.Business.Services.ProductGroupService>();
builder.Services.AddScoped<BmWms.Business.Services.IProductService, BmWms.Business.Services.ProductService>();

// ── JWT Authentication ────────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// ── Rate Limiting (5 request/phút/IP cho endpoint auth) ───────────────────
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 5,
                QueueLimit = 0
            }));
});

// 2. Nạp cấu hình chuỗi kết nối thực thể cơ sở dữ liệu
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// SAU (đúng thứ tự)
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();      // ← chuyển lên TRƯỚC Authentication
app.UseAuthentication();
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