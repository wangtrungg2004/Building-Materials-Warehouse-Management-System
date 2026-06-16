using System;
using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; } = null!;
        public string WarehouseName { get; set; } = null!;
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public User Creator { get; set; } = null!;
        public ICollection<StorageLocation> StorageLocations { get; set; } = new List<StorageLocation>();
    }
}