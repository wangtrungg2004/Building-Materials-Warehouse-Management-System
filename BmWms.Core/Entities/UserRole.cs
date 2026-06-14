using System;

namespace BmWms.Core.Entities
{
    public class UserRole
    {
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public int RoleID { get; set; }
        public Role Role { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}