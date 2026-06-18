using System.ComponentModel.DataAnnotations;

namespace BmWms.Web.DTOs;

// ── Response ─────────────────────────────────────────────────────────
public class ProductGroupListResponse
{
    public int ProductGroupID { get; set; }
    public string GroupCode { get; set; } = "";
    public string GroupName { get; set; } = "";
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// ── Create Request ───────────────────────────────────────────────────
public class CreateProductGroupRequest
{
    [Required(ErrorMessage = "Mã nhóm vật tư là bắt buộc.")]
    [MaxLength(50)]
    public string GroupCode { get; set; } = "";

    [Required(ErrorMessage = "Tên nhóm vật tư là bắt buộc.")]
    [MaxLength(100)]
    public string GroupName { get; set; } = "";

    [MaxLength(500)]
    public string? Description { get; set; }
}

// ── Update Request ───────────────────────────────────────────────────
public class UpdateProductGroupRequest
{
    [Required(ErrorMessage = "Mã nhóm vật tư là bắt buộc.")]
    [MaxLength(50)]
    public string GroupCode { get; set; } = "";

    [Required(ErrorMessage = "Tên nhóm vật tư là bắt buộc.")]
    [MaxLength(100)]
    public string GroupName { get; set; } = "";

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
