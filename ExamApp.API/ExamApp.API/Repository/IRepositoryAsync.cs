using System.Linq.Expressions;
using ExamApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.API.Repository;

public interface IRepositoryAsync<T> where T : BaseEntity
{
    Task BeginTransaction();
    Task Commit();
    Task Rollback();
    IQueryable<T> GetQuery();
    IQueryable<T> GetQueryNoTracking();
    DbSet<T> GetTable();
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeString = null,
        bool disableTracking = true);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        List<Expression<Func<T, object>>> includes = null,
        bool disableTracking = true);
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> AddRangeAsync(List<T> entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(List<T> entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(List<T> entities, CancellationToken cancellationToken = default);
}