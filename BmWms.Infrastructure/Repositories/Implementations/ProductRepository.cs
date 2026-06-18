using Microsoft.EntityFrameworkCore;
using BmWms.Core.Entities;
using BmWms.Infrastructure.Data;
using BmWms.Infrastructure.Repositories.Interfaces;

namespace BmWms.Infrastructure.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Product> Items, int TotalCount)> GetAllAsync(
        string? keyword, int? groupId, bool? isActive, string? unit, int page, int pageSize)
    {
        var query = _context.Products
            .Include(p => p.ProductGroup)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(p =>
                p.ProductCode.ToLower().Contains(kw) ||
                p.ProductName.ToLower().Contains(kw) ||
                (p.SKU != null && p.SKU.ToLower().Contains(kw)) ||
                (p.Barcode != null && p.Barcode.ToLower().Contains(kw)));
        }

        if (groupId.HasValue)
            query = query.Where(p => p.ProductGroupID == groupId.Value);

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(unit))
            query = query.Where(p => p.UnitOfMeasure == unit);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(p => p.UpdatedAt ?? p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.ProductGroup)
            .FirstOrDefaultAsync(p => p.ProductID == id);
    }

    public async Task<Product?> GetByCodeAsync(string code)
    {
        return await _context.Products
            .Include(p => p.ProductGroup)
            .FirstOrDefaultAsync(p => p.ProductCode == code);
    }

    public async Task<bool> ExistsCodeAsync(string code, int? excludeId = null)
    {
        var query = _context.Products.Where(p => p.ProductCode == code);
        if (excludeId.HasValue)
            query = query.Where(p => p.ProductID != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<Product> CreateAsync(Product entity)
    {
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Product> UpdateAsync(Product entity)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity == null) return false;

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<string>> GetDistinctUnitsAsync()
    {
        return await _context.Products
            .Select(p => p.UnitOfMeasure)
            .Distinct()
            .OrderBy(u => u)
            .ToListAsync();
    }
}
