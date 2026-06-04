using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using WMS.Application;
using WMS.Domain.Common;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories;

public class CommonRepository<T> : ICommonRepository<T> where T : BaseEntity
{
    private readonly WmsDbContext _context;
    private readonly DbSet<T> _dbSet;

    public CommonRepository(WmsDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(predicate, ct);

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.ToListAsync(ct);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.Where(predicate).ToListAsync(ct);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.AnyAsync(predicate, ct);

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(T entity, CancellationToken ct = default)
    {

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken ct = default)
    {
        return await _dbSet
        .Where(predicate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync(ct);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbSet.CountAsync(predicate, ct); ;
    }
    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<int> SoftDeleteMultipleAsync(
       Expression<Func<T, bool>> predicate,
       long deletedBy,
       CancellationToken ct = default)
    {
        var utcNow = DateTime.UtcNow;

        return await _dbSet
            .Where(predicate)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.DeletedAt, utcNow)
                .SetProperty(x => x.DeletedBy, deletedBy),
                ct);
    }
    public async Task<IEnumerable<T>> AddRangeAsync(
        IEnumerable<T> entities,
        CancellationToken ct = default)
    {
        await _dbSet.AddRangeAsync(entities, ct);

        await _context.SaveChangesAsync(ct);

        return entities;
    }
    public async Task UpdateRangeAsync(
        IEnumerable<T> entities,
        CancellationToken ct = default)
    {
        _dbSet.UpdateRange(entities);

        await _context.SaveChangesAsync(ct);
    }
}