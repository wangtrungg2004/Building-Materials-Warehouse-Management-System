using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class ProductGroup
    {
        public int ProductGroupID { get; set; }
        public string GroupCode { get; set; } = null!;       // VD: "CEMENT", "STEEL", "BRICK"
        public string GroupName { get; set; } = null!;       // VD: "Xi măng", "Thép", "Gạch"
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
