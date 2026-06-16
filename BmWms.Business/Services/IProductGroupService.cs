using BmWms.Core.Entities;

namespace BmWms.Business.Services;

public interface IProductGroupService
{
    Task<(List<ProductGroup> Items, int TotalCount)> GetAllAsync(string? search, bool? isActive, int page, int pageSize);
    Task<List<ProductGroup>> GetAllActiveAsync();
    Task<ProductGroup?> GetByIdAsync(int id);
    Task<(ProductGroup? Result, string? Error)> CreateAsync(string groupCode, string groupName, string? description, string? createdBy);
    Task<(ProductGroup? Result, string? Error)> UpdateAsync(int id, string groupCode, string groupName, string? description, bool isActive);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
}
