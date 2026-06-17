using BmWms.Infrastructure.DTOs;
using BmWms.Infrastructure.Repositories;

namespace BmWms.Infrastructure.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllWarehousesAsync()
        {
            return await _warehouseRepository.GetWarehouseListAsync();
        }
    }
}
