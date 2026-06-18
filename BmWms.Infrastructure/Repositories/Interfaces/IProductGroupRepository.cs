using BmWms.Core.Entities;

namespace BmWms.Infrastructure.Repositories.Interfaces;

public interface IProductGroupRepository
{
    Task<(List<ProductGroup> Items, int TotalCount)> GetAllAsync(string? search, bool? isActive, int page, int pageSize);
    Task<List<ProductGroup>> GetAllActiveAsync();
    Task<ProductGroup?> GetByIdAsync(int id);
    Task<ProductGroup?> GetByCodeAsync(string code);
    Task<bool> ExistsCodeAsync(string code, int? excludeId = null);
    Task<ProductGroup> CreateAsync(ProductGroup entity);
    Task<ProductGroup> UpdateAsync(ProductGroup entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> HasProductsAsync(int groupId);
}
