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

     
    }
}
