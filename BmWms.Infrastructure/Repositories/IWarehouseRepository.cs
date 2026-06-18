using BmWms.Core.Entities;
using BmWms.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Infrastructure.Repositories
{
    public interface IWarehouseRepository : IGenericRepository<Warehouse>
    {
        /// <summary>
        /// Truy vấn danh sách kho cùng với số lượng Location đếm được (Chưa thực thi Skip/Take để Service xử lý)
        /// </summary>
        IQueryable<WarehouseWithCountProjection> GetWarehouseWithLocationCountQuery();
    }

    // Lớp Projection tạm thời để tránh phụ thuộc vào DTO của tầng Service (Đảm bảo tính độc lập giữa các Layer)
    public class WarehouseWithCountProjection
    {
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string Status { get; set; } = "Active";
        public int TotalLocations { get; set; }
    }
}
