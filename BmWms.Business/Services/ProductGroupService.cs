using BmWms.Core.Entities;
using BmWms.Infrastructure.Repositories.Interfaces;

namespace BmWms.Business.Services;

public class ProductGroupService : IProductGroupService
{
    private readonly IProductGroupRepository _repo;

    public ProductGroupService(IProductGroupRepository repo)
    {
        _repo = repo;
    }

    public Task<(List<ProductGroup> Items, int TotalCount)> GetAllAsync(
        string? search, bool? isActive, int page, int pageSize)
    {
        return _repo.GetAllAsync(search, isActive, page, pageSize);
    }

    public Task<List<ProductGroup>> GetAllActiveAsync()
    {
        return _repo.GetAllActiveAsync();
    }

    public Task<ProductGroup?> GetByIdAsync(int id)
    {
        return _repo.GetByIdAsync(id);
    }

    public async Task<(ProductGroup? Result, string? Error)> CreateAsync(
        string groupCode, string groupName, string? description, string? createdBy)
    {
        // Validate mã nhóm không trùng
        if (await _repo.ExistsCodeAsync(groupCode))
            return (null, $"Mã nhóm '{groupCode}' đã tồn tại.");

        var entity = new ProductGroup
        {
            GroupCode = groupCode.Trim().ToUpper(),
            GroupName = groupName.Trim(),
            Description = description?.Trim(),
            CreatedBy = createdBy,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _repo.CreateAsync(entity);
        return (result, null);
    }

    public async Task<(ProductGroup? Result, string? Error)> UpdateAsync(
        int id, string groupCode, string groupName, string? description, bool isActive)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            return (null, "Không tìm thấy nhóm vật tư.");

        // Validate mã nhóm không trùng (trừ chính nó)
        if (await _repo.ExistsCodeAsync(groupCode, id))
            return (null, $"Mã nhóm '{groupCode}' đã tồn tại.");

        entity.GroupCode = groupCode.Trim().ToUpper();
        entity.GroupName = groupName.Trim();
        entity.Description = description?.Trim();
        entity.IsActive = isActive;
        entity.UpdatedAt = DateTime.UtcNow;

        var result = await _repo.UpdateAsync(entity);
        return (result, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            return (false, "Không tìm thấy nhóm vật tư.");

        // Kiểm tra còn sản phẩm trong nhóm không
        if (await _repo.HasProductsAsync(id))
            return (false, "Không thể xóa nhóm vật tư đang có sản phẩm. Hãy chuyển sản phẩm sang nhóm khác trước.");

        var success = await _repo.DeleteAsync(id);
        return (success, success ? null : "Xóa thất bại.");
    }
}
