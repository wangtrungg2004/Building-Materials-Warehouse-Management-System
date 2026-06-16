using Microsoft.AspNetCore.Mvc;
using BmWms.Business.Services;
using BmWms.Web.DTOs;

namespace BmWms.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductGroupController : ControllerBase
{
    private readonly IProductGroupService _service;

    public ProductGroupController(IProductGroupService service)
    {
        _service = service;
    }

    /// <summary>GET /api/productgroup?search=&isActive=&page=1&pageSize=10</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (items, totalCount) = await _service.GetAllAsync(search, isActive, page, pageSize);

        var response = items.Select(g => new ProductGroupListResponse
        {
            ProductGroupID = g.ProductGroupID,
            GroupCode = g.GroupCode,
            GroupName = g.GroupName,
            Description = g.Description,
            IsActive = g.IsActive,
            CreatedBy = g.CreatedBy,
            CreatedAt = g.CreatedAt,
            UpdatedAt = g.UpdatedAt
        });

        return Ok(new
        {
            items = response,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    /// <summary>GET /api/productgroup/active — dropdown dùng</summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetAllActive()
    {
        var items = await _service.GetAllActiveAsync();
        return Ok(items.Select(g => new { g.ProductGroupID, g.GroupCode, g.GroupName }));
    }

    /// <summary>GET /api/productgroup/{id}</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null)
            return NotFound(new { message = "Không tìm thấy nhóm vật tư." });

        return Ok(new ProductGroupListResponse
        {
            ProductGroupID = entity.ProductGroupID,
            GroupCode = entity.GroupCode,
            GroupName = entity.GroupName,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        });
    }

    /// <summary>POST /api/productgroup</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductGroupRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.CreateAsync(
            request.GroupCode, request.GroupName, request.Description, "System");

        if (error != null)
            return BadRequest(new { message = error });

        return CreatedAtAction(nameof(GetById), new { id = result!.ProductGroupID }, new
        {
            productGroupID = result.ProductGroupID,
            groupCode = result.GroupCode,
            groupName = result.GroupName
        });
    }

    /// <summary>PUT /api/productgroup/{id}</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductGroupRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.UpdateAsync(
            id, request.GroupCode, request.GroupName, request.Description, request.IsActive);

        if (error != null)
            return BadRequest(new { message = error });

        return Ok(new { message = "Cập nhật thành công.", productGroupID = result!.ProductGroupID });
    }

    /// <summary>DELETE /api/productgroup/{id}</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _service.DeleteAsync(id);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Xóa nhóm vật tư thành công." });
    }
}
