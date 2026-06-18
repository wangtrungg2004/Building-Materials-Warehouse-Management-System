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
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AttributeValueDto>? Values { get; set; }
}

public class AttributeDetailResponse
{
    public int AttributeID { get; set; }
    public string AttributeCode { get; set; } = "";
    public string AttributeName { get; set; } = "";
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<AttributeValueDto>? Values { get; set; }
}

public class CreateAttributeRequest
{
    [Required(ErrorMessage = "Mã thuộc tính là bắt buộc.")]
    [MaxLength(50)]
    public string AttributeCode { get; set; } = "";

    [Required(ErrorMessage = "Tên thuộc tính là bắt buộc.")]
    [MaxLength(100)]
    public string AttributeName { get; set; } = "";

    [MaxLength(500)]
    public string? Description { get; set; }
}

public class UpdateAttributeRequest
{
    [Required(ErrorMessage = "Mã thuộc tính là bắt buộc.")]
    [MaxLength(50)]
    public string AttributeCode { get; set; } = "";

    [Required(ErrorMessage = "Tên thuộc tính là bắt buộc.")]
    [MaxLength(100)]
    public string AttributeName { get; set; } = "";

    [MaxLength(500)]
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
}

// ══════════════════════════════════════════════════════════════
// ATTRIBUTE VALUE DTOs
// ══════════════════════════════════════════════════════════════

public class AttributeValueDto
{
    public int ValueID { get; set; }
    public int AttributeID { get; set; }
    public string ValueName { get; set; } = "";
    public bool IsActive { get; set; }
}

public class AttributeValueListResponse
{
    public int ValueID { get; set; }
    public int AttributeID { get; set; }
    public string ValueName { get; set; } = "";
    public bool IsActive { get; set; }
}

public class CreateAttributeValueRequest
{
    [Required(ErrorMessage = "Tên giá trị là bắt buộc.")]
    [MaxLength(100)]
    public string ValueName { get; set; } = "";
}

public class UpdateAttributeValueRequest
{
    [Required(ErrorMessage = "Tên giá trị là bắt buộc.")]
    [MaxLength(100)]
    public string ValueName { get; set; } = "";
    
    public bool IsActive { get; set; } = true;
}

// ══════════════════════════════════════════════════════════════
// PRODUCT ATTRIBUTE SELECTION DTOs
// ══════════════════════════════════════════════════════════════

public class ProductAttributeSelectionDto
{
    public int SelectionID { get; set; }
    public int ProductID { get; set; }
    public int ValueID { get; set; }
    public string ValueName { get; set; } = "";
    public int AttributeID { get; set; }
    public string AttributeName { get; set; } = "";
}

public class ProductAttributesSummaryDto
{
    public int ProductID { get; set; }
    public List<AttributeGroupSummary> Attributes { get; set; } = new();
}

public class AttributeGroupSummary
{
    public int AttributeID { get; set; }
    public string AttributeName { get; set; } = "";
    public List<ValueSummary> Values { get; set; } = new();
}

public class ValueSummary
{
    public int SelectionID { get; set; }
    public int ValueID { get; set; }
    public string ValueName { get; set; } = "";
}

public class SetSelectionsRequest
{
    public List<int> ValueIDs { get; set; } = new();
}

// ══════════════════════════════════════════════════════════════
// ACTIVE ATTRIBUTES FOR FORM (with Values)
// ══════════════════════════════════════════════════════════════

public class ActiveAttributeDto
{
    public int AttributeID { get; set; }
    public string AttributeCode { get; set; } = "";
    public string AttributeName { get; set; } = "";
    public string? Description { get; set; }
    public List<ActiveValueDto> Values { get; set; } = new();
}

public class ActiveValueDto
{
    public int ValueID { get; set; }
    public string ValueName { get; set; } = "";
}
