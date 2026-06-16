using System.Collections.Generic;

namespace BmWms.Core.Entities
{
    public class StorageLocation
    {
        public int LocationID { get; set; }
        public int WarehouseID { get; set; }
        public string LocationCode { get; set; } = null!;
        public string LocationName { get; set; } = null!;
        public int? ParentLocationID { get; set; }
        public bool IsOccupied { get; set; } = false;

        // Navigation Properties
        public Warehouse Warehouse { get; set; } = null!;
        public StorageLocation? ParentLocation { get; set; } // Liên kết đệ quy dựng cây thư mục bãi kho bãi
        public ICollection<StorageLocation> SubLocations { get; set; } = new List<StorageLocation>();
    }
}