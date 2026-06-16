using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleCode { get; set; } = null!; // 'ADMIN', 'OPERATOR', 'STAFF'
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}