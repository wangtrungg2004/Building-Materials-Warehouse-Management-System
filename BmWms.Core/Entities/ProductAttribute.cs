using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class ProductAttribute
    {
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string DataType { get; set; }

        public ICollection<ProductAttributeValue> Values { get; set; }
    }
}
