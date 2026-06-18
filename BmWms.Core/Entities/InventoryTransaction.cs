using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class InventoryTransaction
    {
        public long TransactionID { get; set; }
        public int ProductID { get; set; }
        public int WarehouseID { get; set; }
        public string ReferenceNo { get; set; }
        public decimal DeltaQty { get; set; }
        public DateTime Timestamp { get; set; }

        public Inventory Inventory { get; set; }
    }
}
