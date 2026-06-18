using BmWms.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BmWms.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        // GET: api/warehouses
        [HttpGet]
        public async Task<IActionResult> GetWarehouses([FromQuery] WarehouseQueryRequest request)
        {
            var result = await _warehouseService.GetWarehouseListAsync(
                request.Code,
                request.Name,
                request.Status,
                request.Search,
                request.Page,
                request.PageSize
            );

            return Ok(new
            {
                data = result.Data,
                totalCount = result.TotalCount,
                page = request.Page,
                pageSize = request.PageSize
            });
        }
    }
    public class WarehouseQueryRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Search { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}