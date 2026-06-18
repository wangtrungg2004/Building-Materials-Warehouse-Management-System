using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Infrastructure.DTOs
{
    public class WarehouseListDto
    {
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string Status { get; set; } = "Active";
        public int TotalLocations { get; set; }

        // Mocking hoặc bổ sung thông tin Manager theo UI
        public string ManagerName { get; set; } = "N/A";
        public string ManagerPhone { get; set; } = "N/A";
    }
}
