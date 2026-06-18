using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string Status { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}