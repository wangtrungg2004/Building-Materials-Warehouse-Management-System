using System;

namespace BmWms.Core.Entities
{
    public class PasswordResetOtp
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public DateTime ExpiresAt { get; set; } // ĐÃ SỬA: DateTime2 -> DateTime
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ĐÃ SỬA: DateTime2 -> DateTime
        public bool IsUsed { get; set; } = false;
    }
}