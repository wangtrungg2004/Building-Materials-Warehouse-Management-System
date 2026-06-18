using BmWms.Core.Entities;

namespace BmWms.Business.Services;

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

public interface IProductAttributeService
{
    // Attribute Definitions
    Task<(List<ProductAttribute> Items, int TotalCount)> GetAllAsync(string? keyword, bool? isActive, int page, int pageSize);
    Task<ProductAttribute?> GetByIdAsync(int id);
    Task<(ProductAttribute? Result, string? Error)> CreateAsync(string attributeCode, string attributeName, string dataType, string? options, bool isRequired, int displayOrder);
    Task<(ProductAttribute? Result, string? Error)> UpdateAsync(int id, string attributeCode, string attributeName, string dataType, string? options, bool isRequired, int displayOrder, bool isActive);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
    Task<List<ProductAttribute>> GetActiveAttributesAsync();

    // Attribute Values (for Product Detail)
    Task<List<AttributeValueResponse>> GetProductAttributesAsync(int productId);
    Task SaveProductAttributesAsync(int productId, List<SaveAttributeValueRequest> values);
}
