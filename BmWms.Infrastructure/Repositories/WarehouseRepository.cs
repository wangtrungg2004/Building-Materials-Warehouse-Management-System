using BmWms.Core.Entities;
using BmWms.Infrastructure.Data;
using BmWms.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BmWms.Infrastructure.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Warehouse?> GetByIdAsync(int id) => await _context.Warehouses.FindAsync(id);

        public async Task<IEnumerable<Warehouse>> GetAllAsync() => await _context.Warehouses.ToListAsync();

        public IQueryable<Warehouse> GetQueryable() => _context.Warehouses.AsQueryable();

        public IQueryable<WarehouseWithCountProjection> GetWarehouseWithLocationCountQuery()
        {
            return _context.Warehouses
                .Select(w => new WarehouseWithCountProjection
                {
                    WarehouseID = w.WarehouseID,
                    WarehouseCode = w.WarehouseCode,
                    WarehouseName = w.WarehouseName,
                    Address = w.Address,
                    Status = w.Status,
                    TotalLocations = _context.StorageLocations.Count(l => l.WarehouseID == w.WarehouseID)
                });
        }
    }
}
