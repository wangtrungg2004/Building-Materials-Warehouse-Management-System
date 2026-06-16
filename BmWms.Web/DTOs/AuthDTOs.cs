using System.ComponentModel.DataAnnotations;
using BmWms.Web.DTOs.Validation;

namespace BmWms.Web.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Vui lòng nhập tài khoản.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    public string? Password { get; set; }
}

/// <summary>Chỉ ADMIN gọi được — không có public register.</summary>
public class CreateUserRequest
{
    [Required][MaxLength(50)] public string? Username { get; set; }
    [Required][MaxLength(100)] public string? FullName { get; set; }
    [EmailAddress][MaxLength(100)] public string? Email { get; set; }
    [MaxLength(20)] public string? PhoneNumber { get; set; }
    [Required, StrongPassword] public string? Password { get; set; }

    [Required, MinLength(1, ErrorMessage = "Phải chọn ít nhất một vai trò.")]
    public List<string> RoleCodes { get; set; } = [];
}

public class RefreshTokenRequest
{
    [Required] public string RefreshToken { get; set; } = "";
}

public class ChangePasswordRequest
{
    [Required] public string? CurrentPassword { get; set; }
    [Required, StrongPassword] public string? NewPassword { get; set; }
}

public class ForgotPasswordRequest
{
    [Required, EmailAddress] public string Email { get; set; } = "";
}

public class ResetPasswordRequest
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required] public string Otp { get; set; } = "";
    [Required, StrongPassword] public string NewPassword { get; set; } = "";
}