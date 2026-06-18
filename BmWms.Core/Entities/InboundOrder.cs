using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class InboundOrder
    {
        public string InboundNo { get; set; }
        public DateTime InboundDate { get; set; }
        public string Status { get; set; }

        public int SupplierID { get; set; }
        public int CreatedBy { get; set; }

        public Supplier Supplier { get; set; }
        public User CreatedByUser { get; set; }

        public ICollection<InboundOrderDetail> Details { get; set; }
    }
}
