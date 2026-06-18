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
            .Include(a => a.Values.Where(v => v.IsActive))
            .OrderBy(a => a.AttributeID)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<ProductAttribute?> GetByIdAsync(int id)
    {
        return await _context.ProductAttributes.FindAsync(id);
    }

    public async Task<ProductAttribute?> GetByIdWithValuesAsync(int id)
    {
        return await _context.ProductAttributes
            .Include(a => a.Values.Where(v => v.IsActive))
            .FirstOrDefaultAsync(a => a.AttributeID == id);
    }

    public async Task<ProductAttribute?> GetByCodeAsync(string code)
    {
        return await _context.ProductAttributes
            .Include(a => a.Values.Where(v => v.IsActive))
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
            .Include(a => a.Values.Where(v => v.IsActive))
            .Where(a => a.IsActive)
            .OrderBy(a => a.AttributeID)
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

    public async Task<List<ProductAttributeValue>> GetByAttributeIdAsync(int attributeId)
    {
        return await _context.ProductAttributeValues
            .Where(v => v.AttributeID == attributeId && v.IsActive)
            .ToListAsync();
    }

    public async Task<ProductAttributeValue?> GetByIdAsync(int id)
    {
        return await _context.ProductAttributeValues.FindAsync(id);
    }

    public async Task<ProductAttributeValue> CreateAsync(ProductAttributeValue entity)
    {
        _context.ProductAttributeValues.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<ProductAttributeValue> UpdateAsync(ProductAttributeValue entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ProductAttributeValues.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ProductAttributeValues.FindAsync(id);
        if (entity == null) return false;

        _context.ProductAttributeValues.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ProductAttributeSelectionRepository : IProductAttributeSelectionRepository
{
    private readonly ApplicationDbContext _context;

    public ProductAttributeSelectionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductAttributeSelection>> GetByProductIdAsync(int productId)
    {
        return await _context.ProductAttributeSelections
            .Include(s => s.Value)
                .ThenInclude(v => v.Attribute)
            .Where(s => s.ProductID == productId)
            .ToListAsync();
    }

    public async Task<ProductAttributeSelection> CreateAsync(ProductAttributeSelection entity)
    {
        _context.ProductAttributeSelections.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int selectionId)
    {
        var entity = await _context.ProductAttributeSelections.FindAsync(selectionId);
        if (entity == null) return false;

        _context.ProductAttributeSelections.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteByProductIdAsync(int productId)
    {
        var selections = await _context.ProductAttributeSelections
            .Where(s => s.ProductID == productId)
            .ToListAsync();

        _context.ProductAttributeSelections.RemoveRange(selections);
        await _context.SaveChangesAsync();
    }

    public async Task AddSelectionsAsync(int productId, List<int> valueIds)
    {
        foreach (var valueId in valueIds)
        {
            var exists = await _context.ProductAttributeSelections
                .AnyAsync(s => s.ProductID == productId && s.ValueID == valueId);
            
            if (!exists)
            {
                _context.ProductAttributeSelections.Add(new ProductAttributeSelection
                {
                    ProductID = productId,
                    ValueID = valueId,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task SetSelectionsAsync(int productId, List<int> valueIds)
    {
        var existing = await _context.ProductAttributeSelections
            .Where(s => s.ProductID == productId)
            .ToListAsync();

        _context.ProductAttributeSelections.RemoveRange(existing);

        foreach (var valueId in valueIds)
        {
            _context.ProductAttributeSelections.Add(new ProductAttributeSelection
            {
                ProductID = productId,
                ValueID = valueId,
                CreatedAt = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();
    }
}
