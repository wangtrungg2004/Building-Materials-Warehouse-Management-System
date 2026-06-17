using Microsoft.AspNetCore.Mvc;
using BmWms.Business.Services;

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

        var response = items.Select(a => new
        {
            attributeID = a.AttributeID,
            attributeCode = a.AttributeCode,
            attributeName = a.AttributeName,
            dataType = a.DataType,
            options = a.Options,
            isRequired = a.IsRequired,
            displayOrder = a.DisplayOrder,
            isActive = a.IsActive,
            createdAt = a.CreatedAt
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
        var a = await _service.GetByIdAsync(id);
        if (a == null)
            return NotFound(new { message = "Không tìm thấy thuộc tính." });

        return Ok(new
        {
            attributeID = a.AttributeID,
            attributeCode = a.AttributeCode,
            attributeName = a.AttributeName,
            dataType = a.DataType,
            options = a.Options,
            isRequired = a.IsRequired,
            displayOrder = a.DisplayOrder,
            isActive = a.IsActive,
            createdAt = a.CreatedAt,
            updatedAt = a.UpdatedAt
        });
    }

    /// <summary>POST /api/productattribute</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAttributeRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.CreateAsync(
            request.AttributeCode, request.AttributeName, request.DataType,
            request.Options, request.IsRequired, request.DisplayOrder);

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
            id, request.AttributeCode, request.AttributeName, request.DataType,
            request.Options, request.IsRequired, request.DisplayOrder, request.IsActive);

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

    /// <summary>GET /api/productattribute/product/{productId} — lấy attributes của sản phẩm</summary>
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProductAttributes(int productId)
    {
        var result = await _service.GetProductAttributesAsync(productId);
        return Ok(result);
    }

    /// <summary>GET /api/productattribute/active — lấy danh sách attributes active cho form</summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveForForm()
    {
        var attrs = await _service.GetActiveAttributesAsync();
        return Ok(attrs.Select(a => new
        {
            attributeID = a.AttributeID,
            attributeCode = a.AttributeCode,
            attributeName = a.AttributeName,
            dataType = a.DataType,
            options = a.Options,
            isRequired = a.IsRequired,
            displayOrder = a.DisplayOrder
        }).OrderBy(a => a.displayOrder));
    }

    /// <summary>POST /api/productattribute/product/{productId}/values — lưu attributes của sản phẩm</summary>
    [HttpPost("product/{productId}/values")]
    public async Task<IActionResult> SaveProductAttributes(int productId, [FromBody] Business.Services.SaveProductAttributesRequest request)
    {
        if (request.ProductID != productId)
            return BadRequest(new { message = "ProductID không khớp." });

        await _service.SaveProductAttributesAsync(productId, request.Values);
        return Ok(new { message = "Lưu thuộc tính thành công." });
    }
}

// ── Request DTOs ──────────────────────────────────────────────────────────
public class CreateAttributeRequest
{
    public string AttributeCode { get; set; } = "";
    public string AttributeName { get; set; } = "";
    public string DataType { get; set; } = "Text";
    public string? Options { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateAttributeRequest
{
    public string AttributeCode { get; set; } = "";
    public string AttributeName { get; set; } = "";
    public string DataType { get; set; } = "Text";
    public string? Options { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
