using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class GoodsIssue
    {
        public int GIN_ID { get; set; }
        public string GIN_Number { get; set; } = null!; // Số phiếu: e.g., 'GIN-2026-001'
        public int WarehouseID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Warehouse Warehouse { get; set; } = null!;
        public User Creator { get; set; } = null!;
        public ICollection<GoodsIssueDetail> GoodsIssueDetails { get; set; } = new List<GoodsIssueDetail>();
    }
}