using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Infrastructure.DTOs
{
    public class WarehouseDto
    {
        public string StorageLocationCode { get; set; } = string.Empty;
        public int TotalProducts { get; set; }
        public decimal TotalPhysicalQty { get; set; }
    }
}
