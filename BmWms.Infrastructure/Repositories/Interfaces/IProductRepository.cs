using BmWms.Core.Entities;

namespace BmWms.Infrastructure.Repositories.Interfaces;

public interface IProductRepository
{
    Task<(List<Product> Items, int TotalCount)> GetAllAsync(
        string? keyword, int? groupId, bool? isActive, string? unit, int page, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByCodeAsync(string code);
    Task<bool> ExistsCodeAsync(string code, int? excludeId = null);
    Task<Product> CreateAsync(Product entity);
    Task<Product> UpdateAsync(Product entity);
    Task<bool> DeleteAsync(int id);
    Task<List<string>> GetDistinctUnitsAsync();
}
