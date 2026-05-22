using System.Linq.Expressions;

namespace WMS.Application;

public interface ICommonRepository<T> where T : class
{

    Task<T?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task SoftDeleteAsync(T entity, CancellationToken ct = default);
    Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<int> SoftDeleteMultipleAsync(Expression<Func<T, bool>> predicate,long deletedBy,CancellationToken ct = default);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    IQueryable<T> Query();
}