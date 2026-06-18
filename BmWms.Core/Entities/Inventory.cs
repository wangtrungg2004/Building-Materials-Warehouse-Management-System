using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class Inventory
    {
        public int ProductID { get; set; }
        public int WarehouseID { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReservedQuantity { get; set; }

        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
