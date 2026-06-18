using System.ComponentModel.DataAnnotations;

namespace BmWms.Web.DTOs;

/// <summary>Response trả về thông tin profile của user đang đăng nhập.</summary>
public class UserProfileResponse
{
    public string EmployeeId  { get; set; } = "";   // "EMP" + UserID
    public string Username    { get; set; } = "";
    public string FullName    { get; set; } = "";
    public string? Email      { get; set; }
    public string? PhoneNumber{ get; set; }
    public string Department  { get; set; } = "";   // Lấy từ RoleName đầu tiên
    public string Role        { get; set; } = "";   // RoleCode đầu tiên
    public string JoinDate    { get; set; } = "";   // CreatedAt định dạng dd/MM/yyyy
    public string Status      { get; set; } = "";   // "Active" / "Inactive"
}

/// <summary>Request body cho PUT /api/users/me — chỉ cho phép sửa các trường không Read-Only.</summary>
public class UpdateProfileRequest
{
    [Required(ErrorMessage = "Họ tên không được để trống.")]
    [MaxLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự.")]
    public string FullName { get; set; } = "";

    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20, ErrorMessage = "Số điện thoại tối đa 20 ký tự.")]
    public string? PhoneNumber { get; set; }
}
