using BmWms.Core.Entities;

namespace BmWms.Business.Services;

public interface IProductAttributeService
{
    Task<(List<ProductAttribute> Items, int TotalCount)> GetAllAsync(string? keyword, bool? isActive, int page, int pageSize);
    Task<ProductAttribute?> GetByIdAsync(int id);
    Task<ProductAttribute?> GetByIdWithValuesAsync(int id);
    Task<(ProductAttribute? Result, string? Error)> CreateAsync(string attributeCode, string attributeName, string? description);
    Task<(ProductAttribute? Result, string? Error)> UpdateAsync(int id, string attributeCode, string attributeName, string? description, bool isActive);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
    Task<List<ProductAttribute>> GetActiveAttributesAsync();
}

public interface IProductAttributeValueService
{
    Task<List<ProductAttributeValue>> GetByAttributeIdAsync(int attributeId);
    Task<ProductAttributeValue?> GetByIdAsync(int id);
    Task<(ProductAttributeValue? Result, string? Error)> CreateAsync(int attributeId, string valueName);
    Task<(ProductAttributeValue? Result, string? Error)> UpdateAsync(int id, string valueName, bool isActive);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
}

public interface IProductAttributeSelectionService
{
    Task<List<ProductAttributeSelection>> GetByProductIdAsync(int productId);
    Task<(bool Success, string? Error)> SetSelectionsAsync(int productId, List<int> valueIds);
    Task<(bool Success, string? Error)> AddSelectionsAsync(int productId, List<int> valueIds);
    Task<(bool Success, string? Error)> RemoveSelectionAsync(int selectionId);
}
