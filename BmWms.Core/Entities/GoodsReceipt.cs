using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class GoodsReceipt
    {
        public int GRN_ID { get; set; }
        public string GRN_Number { get; set; } = null!; // Số phiếu: e.g., 'GRN-2026-001'
        public int WarehouseID { get; set; }
        public int SupplierID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Warehouse Warehouse { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;
        public User Creator { get; set; } = null!;
        public ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; } = new List<GoodsReceiptDetail>();
    }
}