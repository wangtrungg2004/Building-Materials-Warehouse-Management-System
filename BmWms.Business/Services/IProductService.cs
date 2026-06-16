using BmWms.Core.Entities;

namespace BmWms.Business.Services;

public interface IProductService
{
    Task<(List<Product> Items, int TotalCount)> GetAllAsync(
        string? keyword, int? groupId, bool? isActive, string? unit, int page, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<(Product? Result, string? Error)> CreateAsync(
        string productCode, string productName, int productGroupId, string unitOfMeasure,
        string? sku, string? barcode, string? brand, string? originCountry,
        string? description, decimal? weight,
        decimal? dimLength, decimal? dimWidth, decimal? dimHeight,
        int? shelfLife, int? warrantyPeriod, string? tags,
        string? imageUrl, decimal minThreshold, string? createdBy);
    Task<(Product? Result, string? Error)> UpdateAsync(
        int id, string productCode, string productName, int productGroupId, string unitOfMeasure,
        string? sku, string? barcode, string? brand, string? originCountry,
        string? description, decimal? weight,
        decimal? dimLength, decimal? dimWidth, decimal? dimHeight,
        int? shelfLife, int? warrantyPeriod, string? tags,
        string? imageUrl, decimal minThreshold, bool isActive);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
    Task<List<string>> GetDistinctUnitsAsync();
}
