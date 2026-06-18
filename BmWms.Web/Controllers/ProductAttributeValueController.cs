using Microsoft.AspNetCore.Mvc;
using BmWms.Business.Services;
using BmWms.Web.DTOs;

namespace BmWms.Web.Controllers;

[Route("api/productattributevalue")]
[ApiController]
public class ProductAttributeValueController : ControllerBase
{
    private readonly IProductAttributeValueService _service;

    public ProductAttributeValueController(IProductAttributeValueService service)
    {
        _service = service;
    }

    /// <summary>GET /api/productattributevalue/by-attribute/{attributeId}</summary>
    [HttpGet("by-attribute/{attributeId}")]
    public async Task<IActionResult> GetByAttribute(int attributeId)
    {
        var values = await _service.GetByAttributeIdAsync(attributeId);
        
        var response = values.Select(v => new AttributeValueListResponse
        {
            ValueID = v.ValueID,
            AttributeID = v.AttributeID,
            ValueName = v.ValueName,
            IsActive = v.IsActive
        });

        return Ok(response);
    }

    /// <summary>GET /api/productattributevalue/{id}</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var v = await _service.GetByIdAsync(id);
        if (v == null)
            return NotFound(new { message = "Không tìm thấy giá trị." });

        var response = new AttributeValueListResponse
        {
            ValueID = v.ValueID,
            AttributeID = v.AttributeID,
            ValueName = v.ValueName,
            IsActive = v.IsActive
        };

        return Ok(response);
    }

    /// <summary>POST /api/productattributevalue?attributeId={attributeId}</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] int attributeId, [FromBody] CreateAttributeValueRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.CreateAsync(attributeId, request.ValueName);

        if (error != null)
            return BadRequest(new { message = error });

        return CreatedAtAction(nameof(GetById), new { id = result!.ValueID }, new
        {
            valueID = result.ValueID,
            valueName = result.ValueName
        });
    }

    /// <summary>PUT /api/productattributevalue/{id}</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAttributeValueRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.UpdateAsync(id, request.ValueName, request.IsActive);

        if (error != null)
            return BadRequest(new { message = error });

        return Ok(new { message = "Cập nhật giá trị thành công.", valueID = result!.ValueID });
    }

    /// <summary>DELETE /api/productattributevalue/{id}</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _service.DeleteAsync(id);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Xóa giá trị thành công." });
    }
}
