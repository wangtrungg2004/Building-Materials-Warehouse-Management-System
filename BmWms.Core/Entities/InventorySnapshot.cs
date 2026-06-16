using System;

namespace BmWms.Core.Entities
{
    public class InventorySnapshot
    {
        public long SnapshotID { get; set; }
        public int WarehouseID { get; set; }
        public int ProductID { get; set; }
        public decimal StoredQty { get; set; }
        public DateTime SnapshotDate { get; set; }

        // Navigation Properties
        public Warehouse Warehouse { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}