using BmWms.Core.Entities;
using BmWms.Infrastructure.Repositories.Interfaces;

namespace BmWms.Business.Services;

public class ProductAttributeService : IProductAttributeService
{
    private readonly IProductAttributeRepository _attrRepo;
    private readonly IProductAttributeValueRepository _valueRepo;

    public ProductAttributeService(
        IProductAttributeRepository attrRepo,
        IProductAttributeValueRepository valueRepo)
    {
        _attrRepo = attrRepo;
        _valueRepo = valueRepo;
    }

    // ── Attribute Definitions ──────────────────────────────────────

    public Task<(List<ProductAttribute> Items, int TotalCount)> GetAllAsync(
        string? keyword, bool? isActive, int page, int pageSize)
    {
        return _attrRepo.GetAllAsync(keyword, isActive, page, pageSize);
    }

    public Task<ProductAttribute?> GetByIdAsync(int id)
    {
        return _attrRepo.GetByIdAsync(id);
    }

    public async Task<(ProductAttribute? Result, string? Error)> CreateAsync(
        string attributeCode, string attributeName, string dataType,
        string? options, bool isRequired, int displayOrder)
    {
        if (await _attrRepo.ExistsCodeAsync(attributeCode))
            return (null, $"Mã thuộc tính '{attributeCode}' đã tồn tại.");

        var entity = new ProductAttribute
        {
            AttributeCode = attributeCode.Trim().ToUpper(),
            AttributeName = attributeName.Trim(),
            DataType = dataType,
            Options = options?.Trim(),
            IsRequired = isRequired,
            DisplayOrder = displayOrder,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _attrRepo.CreateAsync(entity);
        return (result, null);
    }

    public async Task<(ProductAttribute? Result, string? Error)> UpdateAsync(
        int id, string attributeCode, string attributeName, string dataType,
        string? options, bool isRequired, int displayOrder, bool isActive)
    {
        var entity = await _attrRepo.GetByIdAsync(id);
        if (entity == null)
            return (null, "Không tìm thấy thuộc tính.");

        if (await _attrRepo.ExistsCodeAsync(attributeCode, id))
            return (null, $"Mã thuộc tính '{attributeCode}' đã tồn tại.");

        entity.AttributeCode = attributeCode.Trim().ToUpper();
        entity.AttributeName = attributeName.Trim();
        entity.DataType = dataType;
        entity.Options = options?.Trim();
        entity.IsRequired = isRequired;
        entity.DisplayOrder = displayOrder;
        entity.IsActive = isActive;
        entity.UpdatedAt = DateTime.UtcNow;

        var result = await _attrRepo.UpdateAsync(entity);
        return (result, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var entity = await _attrRepo.GetByIdAsync(id);
        if (entity == null)
            return (false, "Không tìm thấy thuộc tính.");

        var success = await _attrRepo.DeleteAsync(id);
        return (success, success ? null : "Xóa thất bại.");
    }

    public Task<List<ProductAttribute>> GetActiveAttributesAsync()
    {
        return _attrRepo.GetActiveAttributesAsync();
    }

    // ── Attribute Values (for Product Detail) ──────────────────────

    public async Task<List<AttributeValueResponse>> GetProductAttributesAsync(int productId)
    {
        var values = await _valueRepo.GetByProductIdAsync(productId);
        var activeAttrs = await _attrRepo.GetActiveAttributesAsync();

        var result = new List<AttributeValueResponse>();

        foreach (var attr in activeAttrs)
        {
            var value = values.FirstOrDefault(v => v.AttributeID == attr.AttributeID);

            result.Add(new AttributeValueResponse
            {
                ValueID = value?.ValueID ?? 0,
                AttributeID = attr.AttributeID,
                AttributeCode = attr.AttributeCode,
                AttributeName = attr.AttributeName,
                DataType = attr.DataType,
                Options = attr.Options,
                IsRequired = attr.IsRequired,
                TextValue = value?.TextValue,
                NumberValue = value?.NumberValue,
                BoolValue = value?.BoolValue,
                DateValue = value?.DateValue,
                DisplayValue = GetDisplayValue(value, attr.DataType)
            });
        }

        return result;
    }

    public async Task SaveProductAttributesAsync(int productId, List<SaveAttributeValueRequest> values)
    {
        foreach (var v in values)
        {
            var entity = new ProductAttributeValue
            {
                TextValue = v.TextValue,
                NumberValue = v.NumberValue,
                BoolValue = v.BoolValue ?? false,
                DateValue = v.DateValue,
                CreatedAt = DateTime.UtcNow
            };

            await _valueRepo.UpsertAsync(productId, v.AttributeID, entity);
        }
    }

    private static string GetDisplayValue(ProductAttributeValue? value, string dataType)
    {
        if (value == null) return "—";

        return dataType switch
        {
            "Text" => value.TextValue ?? "—",
            "Number" => value.NumberValue?.ToString("N0") ?? "—",
            "Decimal" => value.NumberValue?.ToString("N2") ?? "—",
            "Boolean" => value.BoolValue ? "Có" : "Không",
            "Dropdown" => value.TextValue ?? "—",
            "Date" => value.DateValue?.ToString("dd/MM/yyyy") ?? "—",
            _ => value.TextValue ?? "—"
        };
    }
}
