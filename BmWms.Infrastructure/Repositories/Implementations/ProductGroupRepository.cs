using Microsoft.EntityFrameworkCore;
using BmWms.Core.Entities;
using BmWms.Infrastructure.Data;
using BmWms.Infrastructure.Repositories.Interfaces;

namespace BmWms.Infrastructure.Repositories.Implementations;

public class ProductGroupRepository : IProductGroupRepository
{
    private readonly ApplicationDbContext _context;

    public ProductGroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(List<ProductGroup> Items, int TotalCount)> GetAllAsync(
        string? search, bool? isActive, int page, int pageSize)
    {
        var query = _context.ProductGroups.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(g =>
                g.GroupCode.ToLower().Contains(s) ||
                g.GroupName.ToLower().Contains(s) ||
                (g.Description != null && g.Description.ToLower().Contains(s)));
        }

        if (isActive.HasValue)
            query = query.Where(g => g.IsActive == isActive.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(g => g.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<ProductGroup>> GetAllActiveAsync()
    {
        return await _context.ProductGroups
            .Where(g => g.IsActive)
            .OrderBy(g => g.GroupName)
            .ToListAsync();
    }

    public async Task<ProductGroup?> GetByIdAsync(int id)
    {
        return await _context.ProductGroups.FindAsync(id);
    }

    public async Task<ProductGroup?> GetByCodeAsync(string code)
    {
        return await _context.ProductGroups
            .FirstOrDefaultAsync(g => g.GroupCode == code);
    }

    public async Task<bool> ExistsCodeAsync(string code, int? excludeId = null)
    {
        var query = _context.ProductGroups.Where(g => g.GroupCode == code);
        if (excludeId.HasValue)
            query = query.Where(g => g.ProductGroupID != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<ProductGroup> CreateAsync(ProductGroup entity)
    {
        _context.ProductGroups.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<ProductGroup> UpdateAsync(ProductGroup entity)
    {
        _context.ProductGroups.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ProductGroups.FindAsync(id);
        if (entity == null) return false;

        _context.ProductGroups.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasProductsAsync(int groupId)
    {
        return await _context.Products.AnyAsync(p => p.ProductGroupID == groupId);
    }
}
