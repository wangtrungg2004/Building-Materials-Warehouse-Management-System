using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BmWms.Web.Pages.Auth;

/// <summary>
/// Trang đăng nhập — toàn bộ logic xử lý phía client (JavaScript fetch → /api/auth/login).
/// PageModel không cần xử lý gì thêm.
/// </summary>
public class LoginModel : PageModel
{
    public void OnGet() { }
}
