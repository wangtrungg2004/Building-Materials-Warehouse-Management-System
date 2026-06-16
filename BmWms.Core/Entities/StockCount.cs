using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class StockCount
    {
        public int CountID { get; set; }
        public string CountNumber { get; set; } = null!; // Số phiếu: e.g., 'STC-2026-001'
        public int WarehouseID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Warehouse Warehouse { get; set; } = null!;
        public User Creator { get; set; } = null!;
        public ICollection<StockCountDetail> StockCountDetails { get; set; } = new List<StockCountDetail>();
    }
}