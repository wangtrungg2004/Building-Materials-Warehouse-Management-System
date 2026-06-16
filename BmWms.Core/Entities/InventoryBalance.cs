using System;

namespace BmWms.Core.Entities
{
    public class InventoryBalance
    {
        public int BalanceID { get; set; }
        public int ProductID { get; set; }
        public int LocationID { get; set; } // ĐÃ SỬA: Chuỗi mã thô cũ biến thành ID liên kết khóa ngoại cứng

        // Cụm trữ lượng đa trạng thái bảo vệ Realtime hệ thống
        public decimal PhysicalQty { get; set; }
        public decimal AvailableQty { get; set; }
        public decimal CommittedQty { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Product Product { get; set; } = null!;
        public StorageLocation StorageLocation { get; set; } = null!;
    }
}