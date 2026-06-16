using System.ComponentModel.DataAnnotations;

namespace BmWms.Web.DTOs;

// ── List Response ────────────────────────────────────────────────────
public class ProductListResponse
{
    public int ProductID { get; set; }
    public string ProductCode { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string? Description { get; set; }
    public string ProductGroupName { get; set; } = "";
    public int ProductGroupID { get; set; }
    public string UnitOfMeasure { get; set; } = "";
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// ── Detail Response ──────────────────────────────────────────────────
public class ProductDetailResponse
{
    public int ProductID { get; set; }
    public string ProductCode { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string? Description { get; set; }
    public int ProductGroupID { get; set; }
    public string ProductGroupName { get; set; } = "";
    public string UnitOfMeasure { get; set; } = "";
    public string? SKU { get; set; }
    public string? Barcode { get; set; }
    public string? Brand { get; set; }
    public string? OriginCountry { get; set; }
    public decimal? Weight { get; set; }
    public decimal? DimensionLength { get; set; }
    public decimal? DimensionWidth { get; set; }
    public decimal? DimensionHeight { get; set; }
    public int? ShelfLife { get; set; }
    public int? WarrantyPeriod { get; set; }
    public string? Tags { get; set; }
    public string? ImageUrl { get; set; }
    public decimal MinThreshold { get; set; }
    public bool IsActive { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// ── Create Request ───────────────────────────────────────────────────
public class CreateProductRequest
{
    [Required(ErrorMessage = "Mã sản phẩm là bắt buộc.")]
    [MaxLength(50)]
    public string ProductCode { get; set; } = "";

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [MaxLength(200)]
    public string ProductName { get; set; } = "";

    [Required(ErrorMessage = "Nhóm vật tư là bắt buộc.")]
    public int ProductGroupID { get; set; }

    [Required(ErrorMessage = "Đơn vị tính là bắt buộc.")]
    [MaxLength(20)]
    public string UnitOfMeasure { get; set; } = "";

    [MaxLength(50)]
    public string? SKU { get; set; }

    [MaxLength(50)]
    public string? Barcode { get; set; }

    [MaxLength(100)]
    public string? Brand { get; set; }

    [MaxLength(100)]
    public string? OriginCountry { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal? Weight { get; set; }
    public decimal? DimensionLength { get; set; }
    public decimal? DimensionWidth { get; set; }
    public decimal? DimensionHeight { get; set; }
    public int? ShelfLife { get; set; }
    public int? WarrantyPeriod { get; set; }

    [MaxLength(500)]
    public string? Tags { get; set; }

    public string? ImageUrl { get; set; }
    public decimal MinThreshold { get; set; }
}

// ── Update Request ───────────────────────────────────────────────────
public class UpdateProductRequest
{
    [Required(ErrorMessage = "Mã sản phẩm là bắt buộc.")]
    [MaxLength(50)]
    public string ProductCode { get; set; } = "";

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [MaxLength(200)]
    public string ProductName { get; set; } = "";

    [Required(ErrorMessage = "Nhóm vật tư là bắt buộc.")]
    public int ProductGroupID { get; set; }

    [Required(ErrorMessage = "Đơn vị tính là bắt buộc.")]
    [MaxLength(20)]
    public string UnitOfMeasure { get; set; } = "";

    [MaxLength(50)]
    public string? SKU { get; set; }

    [MaxLength(50)]
    public string? Barcode { get; set; }

    [MaxLength(100)]
    public string? Brand { get; set; }

    [MaxLength(100)]
    public string? OriginCountry { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal? Weight { get; set; }
    public decimal? DimensionLength { get; set; }
    public decimal? DimensionWidth { get; set; }
    public decimal? DimensionHeight { get; set; }
    public int? ShelfLife { get; set; }
    public int? WarrantyPeriod { get; set; }

    [MaxLength(500)]
    public string? Tags { get; set; }

    public string? ImageUrl { get; set; }
    public decimal MinThreshold { get; set; }
    public bool IsActive { get; set; } = true;
}
