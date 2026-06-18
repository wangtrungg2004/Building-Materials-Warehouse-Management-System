using BmWms.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Infrastructure.Services
{
    public interface IWarehouseService
    {
        Task<(IEnumerable<WarehouseListDto> Data, int TotalCount)> GetWarehouseListAsync(
            string? code, string? name, string? status, string? search, int page, int pageSize);
    }
}
