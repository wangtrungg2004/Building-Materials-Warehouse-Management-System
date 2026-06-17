using BmWms.Infrastructure.DTOs;
using BmWms.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BmWms.Web.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class WarehouseController : ControllerBase
    //{
    //    private readonly IWarehouseService _warehouseService;

    //    // Nhận Dependency Injection từ Service
    //    public WarehouseController(IWarehouseService warehouseService)
    //    {
    //        _warehouseService = warehouseService;
    //    }

    //    /// <summary>
    //    /// UC01: View Warehouse List
    //    /// </summary>
    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<WarehouseDto>>> GetWarehouses()
    //    {
    //        var warehouses = await _warehouseService.GetAllWarehousesAsync();
    //        return Ok(warehouses);
    //    }


    //}
}
