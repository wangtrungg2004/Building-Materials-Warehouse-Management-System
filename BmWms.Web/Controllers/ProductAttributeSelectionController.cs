using Microsoft.AspNetCore.Mvc;
using BmWms.Business.Services;
using BmWms.Web.DTOs;

namespace BmWms.Web.Controllers;

[Route("api/productattributeselection")]
[ApiController]
public class ProductAttributeSelectionController : ControllerBase
{
    private readonly IProductAttributeSelectionService _selectionService;
    private readonly IProductAttributeService _attrService;

    public ProductAttributeSelectionController(
        IProductAttributeSelectionService selectionService,
        IProductAttributeService attrService)
    {
        _selectionService = selectionService;
        _attrService = attrService;
    }

    /// <summary>GET /api/productattributeselection/product/{productId}</summary>
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetByProduct(int productId)
    {
        var selections = await _selectionService.GetByProductIdAsync(productId);
        
        var result = selections.Select(s => new ProductAttributeSelectionDto
        {
            SelectionID = s.SelectionID,
            ProductID = s.ProductID,
            ValueID = s.ValueID,
            ValueName = s.Value?.ValueName ?? "",
            AttributeID = s.Value?.AttributeID ?? 0,
            AttributeName = s.Value?.Attribute?.AttributeName ?? ""
        });

        return Ok(result);
    }

    /// <summary>POST /api/productattributeselection/product/{productId}</summary>
    [HttpPost("product/{productId}")]
    public async Task<IActionResult> SetSelections(int productId, [FromBody] SetSelectionsRequest request)
    {
        var (success, error) = await _selectionService.SetSelectionsAsync(productId, request.ValueIDs);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Cập nhật thuộc tính sản phẩm thành công." });
    }

    /// <summary>DELETE /api/productattributeselection/{selectionId}</summary>
    [HttpDelete("{selectionId}")]
    public async Task<IActionResult> RemoveSelection(int selectionId)
    {
        var (success, error) = await _selectionService.RemoveSelectionAsync(selectionId);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Xóa lựa chọn thành công." });
    }
}
