using System.Linq.Expressions;
using ExamApp.API.Entities;
using ExamApp.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExamApp.API.Repository.Concrete;

public class RepositoryBase<T> : IRepositoryAsync<T> where T : BaseEntity, ISoftDeletedEntity
{
    protected readonly AppDbContext _databaseContext;
    private DatabaseFacade Database => _databaseContext.Database;
    private IDbContextTransaction Transaction => Database.CurrentTransaction;


    public RepositoryBase(AppDbContext databaseContext)
    {
        _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    public async Task BeginTransaction()
    {
        if (Transaction is null) await Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        if (Transaction is not null) await Database.CommitTransactionAsync();
    }

    public async Task Rollback()
    {
        if (Transaction is not null) await Database.RollbackTransactionAsync();
    }

    public DbSet<T> GetTable() => _databaseContext.Set<T>();

    public IQueryable<T> GetQuery() => GetTable().Where(x => !x.Deleted).AsQueryable();
    public IQueryable<T> GetQueryNoTracking() => GetTable().Where(x => !x.Deleted).AsNoTracking().AsQueryable();

    public virtual async Task<T> GetByIdAsync(int id) => await GetQuery().Where(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IReadOnlyList<T>> GetAllAsync() => await GetQuery().ToListAsync();

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate) =>
        await GetQuery().Where(predicate).ToListAsync();

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null,
        bool disableTracking = true)
    {
        IQueryable<T> query = GetQuery();
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null,
        bool disableTracking = true)
    {
        IQueryable<T> query = GetQuery();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        GetTable().Add(entity);
        var count = await _databaseContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<int> AddRangeAsync(List<T> entity, CancellationToken cancellationToken = default)
    {
        await BeginTransaction();

        await GetTable().AddRangeAsync(entity);
        var count = await _databaseContext.SaveChangesAsync(cancellationToken);
        
        await Commit();

        return count;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _databaseContext.Entry(entity).State = EntityState.Modified;
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        if (entities.Count == 0)
            return;

        await BeginTransaction();

        _databaseContext.UpdateRange(entities);
        await _databaseContext.SaveChangesAsync(cancellationToken);
        await Commit();
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.Deleted = true;
        GetTable().Update(entity);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        await BeginTransaction();

        entities.ForEach(e => e.Deleted = true);
        _databaseContext.UpdateRange(entities);
        await _databaseContext.SaveChangesAsync(cancellationToken);
        
        await Commit();
    }
}