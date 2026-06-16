using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using BmWms.Core.Entities;
using BmWms.Infrastructure.Data;
using BmWms.Infrastructure.Services;
using BmWms.Web.DTOs;
using System.Security.Claims;

namespace BmWms.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly IOtpService _otpService;

    private static readonly TimeSpan OtpLifetime = TimeSpan.FromMinutes(10);

    public AuthController(
        ApplicationDbContext context,
        IConfiguration configuration,
        ITokenService tokenService,
        IOtpService otpService)
    {
        _context = context;
        _configuration = configuration;
        _tokenService = tokenService;
        _otpService = otpService;
    }

    // POST /api/auth/login
    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var user = await _context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        // Kiểm tra password trước, IsActive sau — tránh lộ trạng thái tài khoản
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });

        if (!user.IsActive)
            return Unauthorized(new { message = "Tài khoản đã bị vô hiệu hoá. Vui lòng liên hệ Admin." });

        var roleCodes = user.UserRoles.Select(ur => ur.Role.RoleCode).ToList();
        var (accessToken, expiresAt) = _tokenService.CreateAccessToken(user, roleCodes);
        var refreshToken = await _tokenService.CreateRefreshTokenAsync(user.UserID);

        return Ok(new { token = accessToken, refreshToken, roles = roleCodes, expires = expiresAt });
    }

    // POST /api/auth/refresh-token
    [HttpPost("refresh-token")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest(new { message = "Thiếu refresh token." });

        var existing = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);
        if (existing is null)
            return Unauthorized(new { message = "Refresh token không hợp lệ hoặc đã hết hạn." });

        if (!existing.User.IsActive)
            return Unauthorized(new { message = "Tài khoản đã bị vô hiệu hoá." });

        var roleCodes = existing.User.UserRoles.Select(ur => ur.Role.RoleCode).ToList();
        var (accessToken, expiresAt) = _tokenService.CreateAccessToken(existing.User, roleCodes);
        var newRefreshToken = await _tokenService.CreateRefreshTokenAsync(existing.UserID);
        await _tokenService.RevokeRefreshTokenAsync(existing, newRefreshToken);

        return Ok(new { token = accessToken, refreshToken = newRefreshToken, roles = roleCodes, expires = expiresAt });
    }

    // POST /api/auth/logout  [Authorize]
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            var existing = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);
            if (existing is not null)
                await _tokenService.RevokeRefreshTokenAsync(existing);
        }
        return Ok(new { message = "Đã đăng xuất." });
    }

    // POST /api/auth/users  [Authorize(Roles = "ADMIN")]
    [HttpPost("users")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            return Conflict(new { message = "Username đã tồn tại." });

        var roles = await _context.Roles
            .Where(r => request.RoleCodes.Contains(r.RoleCode))
            .ToListAsync();

        var invalidCodes = request.RoleCodes.Except(roles.Select(r => r.RoleCode)).ToList();
        if (invalidCodes.Count > 0)
            return BadRequest(new { message = $"RoleCode không tồn tại: {string.Join(", ", invalidCodes)}" });

        var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier) is string sid
            ? int.Parse(sid) : (int?)null;

        var user = new User
        {
            Username = request.Username!,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FullName = request.FullName!,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            CreatedBy = adminId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        foreach (var role in roles)
            _context.UserRoles.Add(new UserRole
            { UserID = user.UserID, RoleID = role.RoleID, AssignedAt = DateTime.UtcNow });

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateUser), new { id = user.UserID }, new
        {
            userID = user.UserID,
            username = user.Username,
            fullName = user.FullName,
            roles = roles.Select(r => r.RoleCode)
        });
    }

    // POST /api/auth/change-password  [Authorize]
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr is null) return Unauthorized();

        var user = await _context.Users.FindAsync(int.Parse(userIdStr));
        if (user is null) return Unauthorized();

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return BadRequest(new { message = "Mật khẩu hiện tại không đúng." });

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        var activeTokens = _context.RefreshTokens
            .Where(t => t.UserID == user.UserID && t.RevokedAt == null);
        foreach (var t in activeTokens) t.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại." });
    }

    // POST /api/auth/forgot-password
    [HttpPost("forgot-password")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        const string genericMsg = "Nếu email tồn tại trong hệ thống, OTP đã được gửi.";
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null || !user.IsActive)
            return Ok(new { message = genericMsg });

        var otp = await _otpService.GenerateAndStoreOtpAsync(request.Email, OtpLifetime);

        try { await SendOtpEmail(user.Email!, user.FullName, otp); }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { message = genericMsg });
    }

    // POST /api/auth/reset-password
    [HttpPost("reset-password")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        if (!await _otpService.ValidateAndConsumeOtpAsync(request.Email, request.Otp))
            return BadRequest(new { message = "OTP không hợp lệ hoặc đã hết hạn." });

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null) return BadRequest(new { message = "Không tìm thấy tài khoản." });

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        var activeTokens = _context.RefreshTokens
            .Where(t => t.UserID == user.UserID && t.RevokedAt == null);
        foreach (var t in activeTokens) t.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Đặt lại mật khẩu thành công." });
    }

    // ── Private helper ────────────────────────────────────────────────────────
    private async Task SendOtpEmail(string toEmail, string displayName, string otp)
    {
        var apiKey = _configuration["Resend:ApiKey"]
            ?? throw new InvalidOperationException("Resend:ApiKey chưa được cấu hình.");
        var fromEmail = _configuration["Resend:From"]
            ?? throw new InvalidOperationException("Resend:From chưa được cấu hình.");

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        var payload = new
        {
            from = fromEmail,
            to = new[] { toEmail },
            subject = "Mã OTP đặt lại mật khẩu - BuildMat WMS",
            html = $@"
                <h2>Xin chào {displayName}</h2>
                <p>Mã OTP của bạn:</p>
                <h1 style='letter-spacing:8px;color:#2563eb'>{otp}</h1>
                <p>Hiệu lực <strong>10 phút</strong>. Không chia sẻ cho ai.</p>
                <hr/><small style='color:gray'>BuildMat WMS — Xuân Thành</small>"
        };

        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(payload),
            System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://api.resend.com/emails", content);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Resend lỗi: {await response.Content.ReadAsStringAsync()}");
    }
}