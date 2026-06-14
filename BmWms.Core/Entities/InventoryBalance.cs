using System;

namespace BmWms.Core.Entities
{
    public class InventoryBalance
    {
        public int BalanceID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; } = null!;

        public string StorageLocationCode { get; set; } = null!; // e.g., 'ZONEA-SLOT1'
        public decimal PhysicalQty { get; set; }
        public decimal AvailableQty { get; set; }
        public decimal CommittedQty { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}