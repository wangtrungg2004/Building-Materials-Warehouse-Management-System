using System;

namespace BmWms.Core.Entities
{
    public class InventoryTransaction
    {
        public long TransactionID { get; set; }
        public int ProductID { get; set; }
        public int LocationID { get; set; }
        public string SourceType { get; set; } = null!; // 'INBOUND', 'OUTBOUND', 'TRANSFER', 'ADJUST'
        public string ReferenceNumber { get; set; } = null!;
        public decimal DeltaQty { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Product Product { get; set; } = null!;
        public StorageLocation StorageLocation { get; set; } = null!;
    }
}