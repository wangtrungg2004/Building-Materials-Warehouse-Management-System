using BmWms.Core.Entities;
using BmWms.Infrastructure.Repositories.Interfaces;

namespace BmWms.Business.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IProductGroupRepository _groupRepo;

    public ProductService(IProductRepository repo, IProductGroupRepository groupRepo)
    {
        _repo = repo;
        _groupRepo = groupRepo;
    }

    public Task<(List<Product> Items, int TotalCount)> GetAllAsync(
        string? keyword, int? groupId, bool? isActive, string? unit, int page, int pageSize)
    {
        return _repo.GetAllAsync(keyword, groupId, isActive, unit, page, pageSize);
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        return _repo.GetByIdAsync(id);
    }

    public async Task<(Product? Result, string? Error)> CreateAsync(
        string productCode, string productName, int productGroupId, string unitOfMeasure,
        string? sku, string? barcode, string? brand, string? originCountry,
        string? description, decimal? weight,
        decimal? dimLength, decimal? dimWidth, decimal? dimHeight,
        int? shelfLife, int? warrantyPeriod, string? tags,
        string? imageUrl, decimal minThreshold, string? createdBy)
    {
        // Validate mã sản phẩm không trùng
        if (await _repo.ExistsCodeAsync(productCode))
            return (null, $"Mã sản phẩm '{productCode}' đã tồn tại.");

        // Validate nhóm vật tư tồn tại
        var group = await _groupRepo.GetByIdAsync(productGroupId);
        if (group == null)
            return (null, "Nhóm vật tư không tồn tại.");

        var entity = new Product
        {
            ProductCode = productCode.Trim().ToUpper(),
            ProductName = productName.Trim(),
            ProductGroupID = productGroupId,
            UnitOfMeasure = unitOfMeasure.Trim(),
            SKU = sku?.Trim(),
            Barcode = barcode?.Trim(),
            Brand = brand?.Trim(),
            OriginCountry = originCountry?.Trim(),
            Description = description?.Trim(),
            Weight = weight,
            DimensionLength = dimLength,
            DimensionWidth = dimWidth,
            DimensionHeight = dimHeight,
            ShelfLife = shelfLife,
            WarrantyPeriod = warrantyPeriod,
            Tags = tags?.Trim(),
            ImageUrl = imageUrl,
            MinThreshold = minThreshold,
            IsActive = true,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _repo.CreateAsync(entity);
        return (result, null);
    }

    public async Task<(Product? Result, string? Error)> UpdateAsync(
        int id, string productCode, string productName, int productGroupId, string unitOfMeasure,
        string? sku, string? barcode, string? brand, string? originCountry,
        string? description, decimal? weight,
        decimal? dimLength, decimal? dimWidth, decimal? dimHeight,
        int? shelfLife, int? warrantyPeriod, string? tags,
        string? imageUrl, decimal minThreshold, bool isActive)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            return (null, "Không tìm thấy sản phẩm.");

        // Validate mã sản phẩm không trùng (trừ chính nó)
        if (await _repo.ExistsCodeAsync(productCode, id))
            return (null, $"Mã sản phẩm '{productCode}' đã tồn tại.");

        // Validate nhóm vật tư tồn tại
        var group = await _groupRepo.GetByIdAsync(productGroupId);
        if (group == null)
            return (null, "Nhóm vật tư không tồn tại.");

        entity.ProductCode = productCode.Trim().ToUpper();
        entity.ProductName = productName.Trim();
        entity.ProductGroupID = productGroupId;
        entity.UnitOfMeasure = unitOfMeasure.Trim();
        entity.SKU = sku?.Trim();
        entity.Barcode = barcode?.Trim();
        entity.Brand = brand?.Trim();
        entity.OriginCountry = originCountry?.Trim();
        entity.Description = description?.Trim();
        entity.Weight = weight;
        entity.DimensionLength = dimLength;
        entity.DimensionWidth = dimWidth;
        entity.DimensionHeight = dimHeight;
        entity.ShelfLife = shelfLife;
        entity.WarrantyPeriod = warrantyPeriod;
        entity.Tags = tags?.Trim();
        entity.ImageUrl = imageUrl;
        entity.MinThreshold = minThreshold;
        entity.IsActive = isActive;
        entity.UpdatedAt = DateTime.UtcNow;

        var result = await _repo.UpdateAsync(entity);
        return (result, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            return (false, "Không tìm thấy sản phẩm.");

        var success = await _repo.DeleteAsync(id);
        return (success, success ? null : "Xóa thất bại.");
    }

    public Task<List<string>> GetDistinctUnitsAsync()
    {
        return _repo.GetDistinctUnitsAsync();
    }
}
