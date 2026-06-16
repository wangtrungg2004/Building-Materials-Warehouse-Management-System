using Microsoft.AspNetCore.Mvc;
using BmWms.Business.Services;
using BmWms.Web.DTOs;

namespace BmWms.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    /// <summary>GET /api/product?keyword=&groupId=&isActive=&unit=&page=1&pageSize=10</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? keyword,
        [FromQuery] int? groupId,
        [FromQuery] bool? isActive,
        [FromQuery] string? unit,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (items, totalCount) = await _service.GetAllAsync(keyword, groupId, isActive, unit, page, pageSize);

        var response = items.Select(p => new ProductListResponse
        {
            ProductID = p.ProductID,
            ProductCode = p.ProductCode,
            ProductName = p.ProductName,
            Description = p.Description,
            ProductGroupName = p.ProductGroup?.GroupName ?? "",
            ProductGroupID = p.ProductGroupID,
            UnitOfMeasure = p.UnitOfMeasure,
            IsActive = p.IsActive,
            ImageUrl = p.ImageUrl,
            CreatedBy = p.CreatedBy,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
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

    /// <summary>GET /api/product/{id}</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _service.GetByIdAsync(id);
        if (p == null)
            return NotFound(new { message = "Không tìm thấy sản phẩm." });

        return Ok(new ProductDetailResponse
        {
            ProductID = p.ProductID,
            ProductCode = p.ProductCode,
            ProductName = p.ProductName,
            Description = p.Description,
            ProductGroupID = p.ProductGroupID,
            ProductGroupName = p.ProductGroup?.GroupName ?? "",
            UnitOfMeasure = p.UnitOfMeasure,
            SKU = p.SKU,
            Barcode = p.Barcode,
            Brand = p.Brand,
            OriginCountry = p.OriginCountry,
            Weight = p.Weight,
            DimensionLength = p.DimensionLength,
            DimensionWidth = p.DimensionWidth,
            DimensionHeight = p.DimensionHeight,
            ShelfLife = p.ShelfLife,
            WarrantyPeriod = p.WarrantyPeriod,
            Tags = p.Tags,
            ImageUrl = p.ImageUrl,
            MinThreshold = p.MinThreshold,
            IsActive = p.IsActive,
            CreatedBy = p.CreatedBy,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });
    }

    /// <summary>POST /api/product</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.CreateAsync(
            request.ProductCode, request.ProductName, request.ProductGroupID, request.UnitOfMeasure,
            request.SKU, request.Barcode, request.Brand, request.OriginCountry,
            request.Description, request.Weight,
            request.DimensionLength, request.DimensionWidth, request.DimensionHeight,
            request.ShelfLife, request.WarrantyPeriod, request.Tags,
            request.ImageUrl, request.MinThreshold, "System");

        if (error != null)
            return BadRequest(new { message = error });

        return CreatedAtAction(nameof(GetById), new { id = result!.ProductID }, new
        {
            productID = result.ProductID,
            productCode = result.ProductCode,
            productName = result.ProductName
        });
    }

    /// <summary>PUT /api/product/{id}</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var (result, error) = await _service.UpdateAsync(
            id, request.ProductCode, request.ProductName, request.ProductGroupID, request.UnitOfMeasure,
            request.SKU, request.Barcode, request.Brand, request.OriginCountry,
            request.Description, request.Weight,
            request.DimensionLength, request.DimensionWidth, request.DimensionHeight,
            request.ShelfLife, request.WarrantyPeriod, request.Tags,
            request.ImageUrl, request.MinThreshold, request.IsActive);

        if (error != null)
            return BadRequest(new { message = error });

        return Ok(new { message = "Cập nhật sản phẩm thành công.", productID = result!.ProductID });
    }

    /// <summary>DELETE /api/product/{id}</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _service.DeleteAsync(id);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Xóa sản phẩm thành công." });
    }

    /// <summary>GET /api/product/units — danh sách đơn vị tính cho dropdown</summary>
    [HttpGet("units")]
    public async Task<IActionResult> GetUnits()
    {
        var units = await _service.GetDistinctUnitsAsync();
        return Ok(units);
    }

    /// <summary>POST /api/product/upload-image — upload ảnh sản phẩm</summary>
    [HttpPost("upload-image")]
    [RequestSizeLimit(2 * 1024 * 1024)] // 2MB
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Không có file nào được chọn." });

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var ext = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(ext))
            return BadRequest(new { message = "Chỉ cho phép file PNG, JPG, JPEG." });

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var imageUrl = $"/uploads/products/{fileName}";
        return Ok(new { imageUrl });
    }
}
