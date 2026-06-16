using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class StockTransfer
    {
        public int TransferID { get; set; }
        public string TransferNumber { get; set; } = null!; // Số phiếu: e.g., 'STF-2026-001'
        public int SourceWarehouseID { get; set; } // Khóa ngoại 1 trỏ về Warehouse (Kho nguồn xuất đi)
        public int DestWarehouseID { get; set; }   // Khóa ngoại 2 trỏ về Warehouse (Kho đích tiếp nhận)
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties (Đã thiết kế để tương thích cấu pháp cấm Cascade Paths)
        public Warehouse SourceWarehouse { get; set; } = null!;
        public Warehouse DestWarehouse { get; set; } = null!;
        public User Creator { get; set; } = null!;
        public ICollection<StockTransferDetail> StockTransferDetails { get; set; } = new List<StockTransferDetail>();
    }
}