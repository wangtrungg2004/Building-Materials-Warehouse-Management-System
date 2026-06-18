using BmWms.Core.Entities;
using BmWms.Infrastructure.Repositories.Interfaces;

namespace BmWms.Business.Services;

public class ProductAttributeService : IProductAttributeService
{
    private readonly IProductAttributeRepository _attrRepo;

    public ProductAttributeService(IProductAttributeRepository attrRepo)
    {
        _attrRepo = attrRepo;
    }

    public Task<(List<ProductAttribute> Items, int TotalCount)> GetAllAsync(
        string? keyword, bool? isActive, int page, int pageSize)
    {
        return _attrRepo.GetAllAsync(keyword, isActive, page, pageSize);
    }

    public Task<ProductAttribute?> GetByIdAsync(int id)
    {
        return _attrRepo.GetByIdAsync(id);
    }

    public Task<ProductAttribute?> GetByIdWithValuesAsync(int id)
    {
        return _attrRepo.GetByIdWithValuesAsync(id);
    }

    public async Task<(ProductAttribute? Result, string? Error)> CreateAsync(
        string attributeCode, string attributeName, string? description)
    {
        if (await _attrRepo.ExistsCodeAsync(attributeCode))
            return (null, $"Mã thuộc tính '{attributeCode}' đã tồn tại.");

        var entity = new ProductAttribute
        {
            AttributeCode = attributeCode.Trim().ToUpper(),
            AttributeName = attributeName.Trim(),
            Description = description?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _attrRepo.CreateAsync(entity);
        return (result, null);
    }

    public async Task<(ProductAttribute? Result, string? Error)> UpdateAsync(
        int id, string attributeCode, string attributeName, string? description, bool isActive)
    {
        var entity = await _attrRepo.GetByIdAsync(id);
        if (entity == null)
            return (null, "Không tìm thấy thuộc tính.");

        if (await _attrRepo.ExistsCodeAsync(attributeCode, id))
            return (null, $"Mã thuộc tính '{attributeCode}' đã tồn tại.");

        entity.AttributeCode = attributeCode.Trim().ToUpper();
        entity.AttributeName = attributeName.Trim();
        entity.Description = description?.Trim();
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
}

public class ProductAttributeValueService : IProductAttributeValueService
{
    private readonly IProductAttributeValueRepository _valueRepo;
    private readonly IProductAttributeRepository _attrRepo;

    public ProductAttributeValueService(
        IProductAttributeValueRepository valueRepo,
        IProductAttributeRepository attrRepo)
    {
        _valueRepo = valueRepo;
        _attrRepo = attrRepo;
    }

    public Task<List<ProductAttributeValue>> GetByAttributeIdAsync(int attributeId)
    {
        return _valueRepo.GetByAttributeIdAsync(attributeId);
    }

    public Task<ProductAttributeValue?> GetByIdAsync(int id)
    {
        return _valueRepo.GetByIdAsync(id);
    }

    public async Task<(ProductAttributeValue? Result, string? Error)> CreateAsync(
        int attributeId, string valueName)
    {
        var attr = await _attrRepo.GetByIdAsync(attributeId);
        if (attr == null)
            return (null, "Không tìm thấy thuộc tính.");

        var entity = new ProductAttributeValue
        {
            AttributeID = attributeId,
            ValueName = valueName.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _valueRepo.CreateAsync(entity);
        return (result, null);
    }

    public async Task<(ProductAttributeValue? Result, string? Error)> UpdateAsync(
        int id, string valueName, bool isActive)
    {
        var entity = await _valueRepo.GetByIdAsync(id);
        if (entity == null)
            return (null, "Không tìm thấy giá trị.");

        entity.ValueName = valueName.Trim();
        entity.IsActive = isActive;
        entity.UpdatedAt = DateTime.UtcNow;

        var result = await _valueRepo.UpdateAsync(entity);
        return (result, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var entity = await _valueRepo.GetByIdAsync(id);
        if (entity == null)
            return (false, "Không tìm thấy giá trị.");

        var success = await _valueRepo.DeleteAsync(id);
        return (success, success ? null : "Xóa thất bại.");
    }
}

public class ProductAttributeSelectionService : IProductAttributeSelectionService
{
    private readonly IProductAttributeSelectionRepository _selectionRepo;

    public ProductAttributeSelectionService(IProductAttributeSelectionRepository selectionRepo)
    {
        _selectionRepo = selectionRepo;
    }

    public async Task<List<ProductAttributeSelection>> GetByProductIdAsync(int productId)
    {
        return await _selectionRepo.GetByProductIdAsync(productId);
    }

    public async Task<(bool Success, string? Error)> SetSelectionsAsync(int productId, List<int> valueIds)
    {
        try
        {
            await _selectionRepo.SetSelectionsAsync(productId, valueIds);
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> AddSelectionsAsync(int productId, List<int> valueIds)
    {
        try
        {
            await _selectionRepo.AddSelectionsAsync(productId, valueIds);
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> RemoveSelectionAsync(int selectionId)
    {
        var success = await _selectionRepo.DeleteAsync(selectionId);
        return (success, success ? null : "Không tìm thấy lựa chọn.");
    }
}
