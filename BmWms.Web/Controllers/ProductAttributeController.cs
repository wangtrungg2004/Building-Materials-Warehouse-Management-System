using Microsoft.AspNetCore.Mvc;
using BmWms.Business.Services;
using BmWms.Web.DTOs;

namespace BmWms.Web.Controllers;

[Route("api/productattribute")]
[ApiController]
public class ProductAttributeController : ControllerBase
{
    private readonly IProductAttributeService _service;

    public ProductAttributeController(IProductAttributeService service)
    {
        _service = service;
    }

    /// <summary>GET /api/productattribute?keyword=&isActive=&page=1&pageSize=10</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? keyword,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (items, totalCount) = await _service.GetAllAsync(keyword, isActive, page, pageSize);

        var response = items.Select(a => new AttributeListResponse
        {
            AttributeID = a.AttributeID,
            AttributeCode = a.AttributeCode,
            AttributeName = a.AttributeName,
            Description = a.Description,
            IsActive = a.IsActive,
            CreatedAt = a.CreatedAt,
            Values = a.Values?.Select(v => new AttributeValueDto
            {
                ValueID = v.ValueID,
                AttributeID = v.AttributeID,
                ValueName = v.ValueName,
                IsActive = v.IsActive
            }).ToList()
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

    /// <summary>GET /api/productattribute/{id}</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var a = await _service.GetByIdWithValuesAsync(id);
        if (a == null)
            return NotFound(new { message = "Không tìm thấy thuộc tính." });

        var response = new AttributeDetailResponse
        {
            AttributeID = a.AttributeID,
            AttributeCode = a.AttributeCode,
            AttributeName = a.AttributeName,
            Description = a.Description,
            IsActive = a.IsActive,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            Values = a.Values?.Select(v => new AttributeValueDto
            {
                ValueID = v.ValueID,
                AttributeID = v.AttributeID,
                ValueName = v.ValueName,
                IsActive = v.IsActive
            }).ToList()
        };

        return Ok(response);
    }

    /// <summary>POST /api/productattribute</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAttributeRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.CreateAsync(
            request.AttributeCode, request.AttributeName, request.Description);

        if (error != null)
            return BadRequest(new { message = error });

        return CreatedAtAction(nameof(GetById), new { id = result!.AttributeID }, new
        {
            attributeID = result.AttributeID,
            attributeCode = result.AttributeCode,
            attributeName = result.AttributeName
        });
    }

    /// <summary>PUT /api/productattribute/{id}</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAttributeRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.UpdateAsync(
            id, request.AttributeCode, request.AttributeName, request.Description, request.IsActive);

        if (error != null)
            return BadRequest(new { message = error });

        return Ok(new { message = "Cập nhật thuộc tính thành công.", attributeID = result!.AttributeID });
    }

    /// <summary>DELETE /api/productattribute/{id}</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _service.DeleteAsync(id);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Xóa thuộc tính thành công." });
    }

    /// <summary>GET /api/productattribute/active — lấy danh sách attributes active cho form</summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveForForm()
    {
        var attrs = await _service.GetActiveAttributesAsync();
        
        var response = attrs.Select(a => new ActiveAttributeDto
        {
            AttributeID = a.AttributeID,
            AttributeCode = a.AttributeCode,
            AttributeName = a.AttributeName,
            Description = a.Description,
            Values = a.Values?.Select(v => new ActiveValueDto
            {
                ValueID = v.ValueID,
                ValueName = v.ValueName,
            }).ToList() ?? new List<ActiveValueDto>()
        });

        return Ok(response);
    }
}
