using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class SupplierProduct
    {
        public int SupplierID { get; set; }
        public int ProductID { get; set; }

        public Supplier Supplier { get; set; }
        public Product Product { get; set; }
    }
}
