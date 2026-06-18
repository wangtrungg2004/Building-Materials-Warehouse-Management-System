using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; }

        public ICollection<StorageLocation> Locations { get; set; }
    }
}
