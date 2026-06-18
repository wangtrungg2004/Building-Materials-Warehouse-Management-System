using System.ComponentModel.DataAnnotations;

namespace BmWms.Web.DTOs;

// ══════════════════════════════════════════════════════════════
// ATTRIBUTE DEFINITION DTOs
// ══════════════════════════════════════════════════════════════

public class AttributeListResponse
{
    public int AttributeID { get; set; }
    public string AttributeCode { get; set; } = "";
    public string AttributeName { get; set; } = "";
    public string DataType { get; set; } = "";
    public string? Options { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AttributeDetailResponse
{
    public int AttributeID { get; set; }
    public string AttributeCode { get; set; } = "";
    public string AttributeName { get; set; } = "";
    public string DataType { get; set; } = "";
    public string? Options { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateAttributeRequest
{
    [Required(ErrorMessage = "Mã thuộc tính là bắt buộc.")]
    [MaxLength(50)]
    public string AttributeCode { get; set; } = "";

    [Required(ErrorMessage = "Tên thuộc tính là bắt buộc.")]
    [MaxLength(100)]
    public string AttributeName { get; set; } = "";

    [Required(ErrorMessage = "Loại dữ liệu là bắt buộc.")]
    public string DataType { get; set; } = "Text"; // Text, Number, Decimal, Boolean, Dropdown, Date

    public string? Options { get; set; } // JSON array for dropdown

    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateAttributeRequest
{
    [Required(ErrorMessage = "Mã thuộc tính là bắt buộc.")]
    [MaxLength(50)]
    public string AttributeCode { get; set; } = "";

    [Required(ErrorMessage = "Tên thuộc tính là bắt buộc.")]
    [MaxLength(100)]
    public string AttributeName { get; set; } = "";

    [Required(ErrorMessage = "Loại dữ liệu là bắt buộc.")]
    public string DataType { get; set; } = "Text";

    public string? Options { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

// ══════════════════════════════════════════════════════════════
// ATTRIBUTE VALUE DTOs (for Product Detail)
// ══════════════════════════════════════════════════════════════

public class AttributeValueResponse
{
    public int ValueID { get; set; }
    public int AttributeID { get; set; }
    public string AttributeCode { get; set; } = "";
    public string AttributeName { get; set; } = "";
    public string DataType { get; set; } = "";
    public string? Options { get; set; }
    public bool IsRequired { get; set; }
    public string? TextValue { get; set; }
    public decimal? NumberValue { get; set; }
    public bool? BoolValue { get; set; }
    public DateTime? DateValue { get; set; }
    public string DisplayValue { get; set; } = "";
}

public class SaveAttributeValueRequest
{
    public int AttributeID { get; set; }
    public string? TextValue { get; set; }
    public decimal? NumberValue { get; set; }
    public bool? BoolValue { get; set; }
    public DateTime? DateValue { get; set; }
}

public class SaveProductAttributesRequest
{
    public int ProductID { get; set; }
    public List<SaveAttributeValueRequest> Values { get; set; } = new();
}
