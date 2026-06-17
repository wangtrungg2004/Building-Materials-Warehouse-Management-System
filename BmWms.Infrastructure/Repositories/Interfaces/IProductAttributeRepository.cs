using BmWms.Core.Entities;

namespace BmWms.Infrastructure.Repositories.Interfaces;

public interface IProductAttributeRepository
{
    // Attribute Definitions
    Task<(List<ProductAttribute> Items, int TotalCount)> GetAllAsync(string? keyword, bool? isActive, int page, int pageSize);
    Task<ProductAttribute?> GetByIdAsync(int id);
    Task<ProductAttribute?> GetByCodeAsync(string code);
    Task<bool> ExistsCodeAsync(string code, int? excludeId = null);
    Task<ProductAttribute> CreateAsync(ProductAttribute entity);
    Task<ProductAttribute> UpdateAsync(ProductAttribute entity);
    Task<bool> DeleteAsync(int id);
    Task<List<ProductAttribute>> GetActiveAttributesAsync();
}

public interface IProductAttributeValueRepository
{
    Task<List<ProductAttributeValue>> GetByProductIdAsync(int productId);
    Task<ProductAttributeValue?> GetByProductAndAttributeAsync(int productId, int attributeId);
    Task<ProductAttributeValue> CreateAsync(ProductAttributeValue entity);
    Task<ProductAttributeValue> UpdateAsync(ProductAttributeValue entity);
    Task<bool> DeleteAsync(int valueId);
    Task DeleteByProductIdAsync(int productId);
    Task UpsertAsync(int productId, int attributeId, ProductAttributeValue value);
}
