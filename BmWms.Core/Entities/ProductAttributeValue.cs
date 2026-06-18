using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class ProductAttributeValue
    {
        public int ProductID { get; set; }
        public int AttributeID { get; set; }
        public string Value { get; set; }

        public Product Product { get; set; }
        public ProductAttribute Attribute { get; set; }
    }
}
