using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Core.Entities
{
    public class ProductGroup
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
