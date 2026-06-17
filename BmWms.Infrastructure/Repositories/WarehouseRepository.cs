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

        public async Task<IEnumerable<WarehouseDto>> GetWarehouseListAsync()
        {
            return await _context.InventoryBalances
                .GroupBy(i => i.StorageLocationCode)
                .Select(g => new WarehouseDto
                {
                    StorageLocationCode = g.Key,
                    TotalProducts = g.Select(p => p.ProductID).Distinct().Count(),
                    TotalPhysicalQty = g.Sum(p => p.PhysicalQty)
                })
                .ToListAsync();
        }
    }
}
