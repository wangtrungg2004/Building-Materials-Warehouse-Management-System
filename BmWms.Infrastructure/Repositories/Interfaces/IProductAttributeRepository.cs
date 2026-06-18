using BmWms.Core.Entities;

namespace BmWms.Infrastructure.Repositories.Interfaces;

public interface IProductAttributeRepository
{
    // Attribute Definitions
    Task<(List<ProductAttribute> Items, int TotalCount)> GetAllAsync(string? keyword, bool? isActive, int page, int pageSize);
    Task<ProductAttribute?> GetByIdAsync(int id);
    Task<ProductAttribute?> GetByIdWithValuesAsync(int id);
    Task<ProductAttribute?> GetByCodeAsync(string code);
    Task<bool> ExistsCodeAsync(string code, int? excludeId = null);
    Task<ProductAttribute> CreateAsync(ProductAttribute entity);
    Task<ProductAttribute> UpdateAsync(ProductAttribute entity);
    Task<bool> DeleteAsync(int id);
    Task<List<ProductAttribute>> GetActiveAttributesAsync();
}

public interface IProductAttributeValueRepository
{
    // Attribute Values
    Task<List<ProductAttributeValue>> GetByAttributeIdAsync(int attributeId);
    Task<ProductAttributeValue?> GetByIdAsync(int id);
    Task<ProductAttributeValue> CreateAsync(ProductAttributeValue entity);
    Task<ProductAttributeValue> UpdateAsync(ProductAttributeValue entity);
    Task<bool> DeleteAsync(int id);
}

public interface IProductAttributeSelectionRepository
{
    // Selections (Product-Value mappings)
    Task<List<ProductAttributeSelection>> GetByProductIdAsync(int productId);
    Task<ProductAttributeSelection> CreateAsync(ProductAttributeSelection entity);
    Task<bool> DeleteAsync(int selectionId);
    Task DeleteByProductIdAsync(int productId);
    Task AddSelectionsAsync(int productId, List<int> valueIds);
    Task SetSelectionsAsync(int productId, List<int> valueIds);
}
