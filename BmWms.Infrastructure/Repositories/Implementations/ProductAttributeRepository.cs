using Microsoft.EntityFrameworkCore;
using BmWms.Core.Entities;
using BmWms.Infrastructure.Data;
using BmWms.Infrastructure.Repositories.Interfaces;

namespace BmWms.Infrastructure.Repositories.Implementations;

public class ProductAttributeRepository : IProductAttributeRepository
{
    private readonly ApplicationDbContext _context;

    public ProductAttributeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(List<ProductAttribute> Items, int TotalCount)> GetAllAsync(
        string? keyword, bool? isActive, int page, int pageSize)
    {
        var query = _context.ProductAttributes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(a =>
                a.AttributeCode.ToLower().Contains(kw) ||
                a.AttributeName.ToLower().Contains(kw));
        }

        if (isActive.HasValue)
            query = query.Where(a => a.IsActive == isActive.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(a => a.DisplayOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<ProductAttribute?> GetByIdAsync(int id)
    {
        return await _context.ProductAttributes.FindAsync(id);
    }

    public async Task<ProductAttribute?> GetByCodeAsync(string code)
    {
        return await _context.ProductAttributes
            .FirstOrDefaultAsync(a => a.AttributeCode == code);
    }

    public async Task<bool> ExistsCodeAsync(string code, int? excludeId = null)
    {
        var query = _context.ProductAttributes.Where(a => a.AttributeCode == code);
        if (excludeId.HasValue)
            query = query.Where(a => a.AttributeID != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<ProductAttribute> CreateAsync(ProductAttribute entity)
    {
        _context.ProductAttributes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<ProductAttribute> UpdateAsync(ProductAttribute entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ProductAttributes.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ProductAttributes.FindAsync(id);
        if (entity == null) return false;

        _context.ProductAttributes.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductAttribute>> GetActiveAttributesAsync()
    {
        return await _context.ProductAttributes
            .Where(a => a.IsActive)
            .OrderBy(a => a.DisplayOrder)
            .ToListAsync();
    }
}

public class ProductAttributeValueRepository : IProductAttributeValueRepository
{
    private readonly ApplicationDbContext _context;

    public ProductAttributeValueRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductAttributeValue>> GetByProductIdAsync(int productId)
    {
        return await _context.ProductAttributeValues
            .Include(v => v.Attribute)
            .Where(v => v.ProductID == productId)
            .OrderBy(v => v.Attribute!.DisplayOrder)
            .ToListAsync();
    }

    public async Task<ProductAttributeValue?> GetByProductAndAttributeAsync(int productId, int attributeId)
    {
        return await _context.ProductAttributeValues
            .FirstOrDefaultAsync(v => v.ProductID == productId && v.AttributeID == attributeId);
    }

    public async Task<ProductAttributeValue> CreateAsync(ProductAttributeValue entity)
    {
        _context.ProductAttributeValues.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<ProductAttributeValue> UpdateAsync(ProductAttributeValue entity)
    {
        _context.ProductAttributeValues.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int valueId)
    {
        var entity = await _context.ProductAttributeValues.FindAsync(valueId);
        if (entity == null) return false;

        _context.ProductAttributeValues.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteByProductIdAsync(int productId)
    {
        var values = await _context.ProductAttributeValues
            .Where(v => v.ProductID == productId)
            .ToListAsync();

        _context.ProductAttributeValues.RemoveRange(values);
        await _context.SaveChangesAsync();
    }

    public async Task UpsertAsync(int productId, int attributeId, ProductAttributeValue value)
    {
        var existing = await GetByProductAndAttributeAsync(productId, attributeId);
        if (existing != null)
        {
            existing.TextValue = value.TextValue;
            existing.NumberValue = value.NumberValue;
            existing.BoolValue = value.BoolValue;
            existing.DateValue = value.DateValue;
            _context.ProductAttributeValues.Update(existing);
        }
        else
        {
            value.ProductID = productId;
            value.AttributeID = attributeId;
            _context.ProductAttributeValues.Add(value);
        }
        await _context.SaveChangesAsync();
    }
}
