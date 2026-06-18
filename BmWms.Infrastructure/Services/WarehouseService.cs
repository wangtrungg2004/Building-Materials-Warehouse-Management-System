using BmWms.Infrastructure.DTOs;
using BmWms.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BmWms.Infrastructure.Services
{
  public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        // Dependency Injection đảo ngược phụ thuộc (DIP) thông qua Interface
        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<(IEnumerable<WarehouseListDto> Data, int TotalCount)> GetWarehouseListAsync(
            string? code, string? name, string? status, string? search, int page, int pageSize)
        {
            // 1. Lấy Query thô từ Repo
            var rawQuery = _warehouseRepository.GetWarehouseWithLocationCountQuery();

            // 2. Xử lý nghiệp vụ Lọc dữ liệu nâng cao (Advanced Filters)
            if (!string.IsNullOrEmpty(code))
                rawQuery = rawQuery.Where(w => w.WarehouseCode.Contains(code));

            if (!string.IsNullOrEmpty(name))
                rawQuery = rawQuery.Where(w => w.WarehouseName.Contains(name));

            if (!string.IsNullOrEmpty(status) && status != "All")
                rawQuery = rawQuery.Where(w => w.Status == status);

            // 3. Xử lý Toàn cục Tìm kiếm (Global Search Box)
            if (!string.IsNullOrEmpty(search))
            {
                rawQuery = rawQuery.Where(w => w.WarehouseCode.Contains(search) 
                                            || w.WarehouseName.Contains(search) 
                                            || (w.Address != null && w.Address.Contains(search)));
            }

            // 4. Lấy tổng số bản ghi trước khi phân trang (phục vụ hiển thị UI: Showing X to Y of Z entries)
            int totalCount = await rawQuery.CountAsync();

            // 5. Sắp xếp, phân trang và Mapping sang DTO chính xác cho giao diện hiển thị
            var processedData = await rawQuery
                .OrderBy(w => w.WarehouseCode)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(item => new WarehouseListDto
                {
                    WarehouseID = item.WarehouseID,
                    WarehouseCode = item.WarehouseCode,
                    WarehouseName = item.WarehouseName,
                    Address = item.Address,
                    Status = item.Status,
                    TotalLocations = item.TotalLocations,
                    
                    // Giả lập dữ liệu quản lý (Vì Schema DB chưa liên kết thực thể Quản lý vào Warehouses)
                    ManagerName = item.WarehouseCode == "WH001" ? "Tran Van B" : 
                                  item.WarehouseCode == "WH002" ? "Le Van C" : 
                                  item.WarehouseCode == "WH003" ? "Phan Van D" : "Hoang Van E",
                    ManagerPhone = item.WarehouseCode == "WH001" ? "0987 654 321" : 
                                   item.WarehouseCode == "WH002" ? "0903 123 456" : 
                                   item.WarehouseCode == "WH003" ? "0912 345 678" : "0934 567 890"
                })
                .ToListAsync();

            return (processedData, totalCount);
        }
    }
}
