using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class InboundOrderDetail
    {
        public string InboundNo { get; set; }
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }

        public InboundOrder InboundOrder { get; set; }
        public Product Product { get; set; }
    }
}
