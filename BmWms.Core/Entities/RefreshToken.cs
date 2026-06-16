using System;

namespace BmWms.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string TokenHash { get; set; } = null!;
        public DateTime ExpiresAt { get; set; } // ĐÃ SỬA: DateTime2 -> DateTime
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ĐÃ SỬA: DateTime2 -> DateTime
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByTokenHash { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;

        // Thuộc tính Logic Runtime (Sẽ cấu hình Ignore trong DbContext, không map xuống DB)
        public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
    }
}