using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BmWms.Infrastructure.Data;
using BmWms.Web.DTOs;
using System.Security.Claims;

namespace BmWms.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/users/me — Lấy thông tin profile user đang đăng nhập
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr is null) return Unauthorized();

        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserID == int.Parse(userIdStr));

        if (user is null) return NotFound(new { message = "Không tìm thấy người dùng." });

        // Lấy role đầu tiên làm Department & Role display
        var firstRole = user.UserRoles
            .OrderBy(ur => ur.AssignedAt)
            .Select(ur => ur.Role)
            .FirstOrDefault();

        var profile = new UserProfileResponse
        {
            EmployeeId   = $"EMP{user.UserID:D3}",
            Username     = user.Username,
            FullName     = user.FullName,
            Email        = user.Email,
            PhoneNumber  = user.PhoneNumber,
            Department   = firstRole?.RoleName ?? "—",
            Role         = firstRole?.RoleCode ?? "—",
            JoinDate     = user.CreatedAt.ToString("dd/MM/yyyy"),
            Status       = user.IsActive ? "Active" : "Inactive"
        };

        return Ok(profile);
    }

    // PUT /api/users/me — Cập nhật profile (chỉ FullName, Email, PhoneNumber)
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr is null) return Unauthorized();

        var user = await _context.Users.FindAsync(int.Parse(userIdStr));
        if (user is null) return NotFound(new { message = "Không tìm thấy người dùng." });

        // Kiểm tra email trùng với user khác
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await _context.Users.AnyAsync(u =>
                u.Email == request.Email && u.UserID != user.UserID);
            if (emailExists)
                return Conflict(new { message = "Email này đã được sử dụng bởi tài khoản khác." });
        }

        user.FullName    = request.FullName.Trim();
        user.Email       = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
        user.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim();
        user.UpdatedAt   = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message     = "Cập nhật thông tin thành công.",
            fullName    = user.FullName,
            email       = user.Email,
            phoneNumber = user.PhoneNumber
        });
    }
}
