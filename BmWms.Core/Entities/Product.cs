using System;

namespace BmWms.Core.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal MinThreshold { get; set; }

        // SỬA TẠI ĐÂY: bit -> bool
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}