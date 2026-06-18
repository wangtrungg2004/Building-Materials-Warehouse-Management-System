using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class StorageLocation
    {
        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string? LocationType { get; set; }
        public decimal? Capacity { get; set; }

        public int WarehouseID { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
