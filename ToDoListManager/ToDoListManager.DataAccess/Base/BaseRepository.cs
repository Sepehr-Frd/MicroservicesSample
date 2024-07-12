using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ToDoListManager.Common.Dtos;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.DataAccess.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    // [Fields]

    private readonly DbSet<T> _dbSet;

    // [Constructor]

    protected BaseRepository(DbContext dbContext)
    {
        _dbSet = dbContext.Set<T>();
    }

    // [Methods]

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var createdEntityEntry = await _dbSet.AddAsync(entity, cancellationToken);

        return createdEntityEntry.Entity;
    }

    public async Task<List<T>> GetAllAsync(
        PaginationDto? paginationDto = null,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        paginationDto ??= PaginationDto.DefaultPaginationDto;

        var skipCount = (paginationDto.PageNumber - 1) * paginationDto.PageSize;

        query = query
            .OrderBy(entity => entity.Id)
            .Skip(skipCount)
            .Take(paginationDto.PageSize);

        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().Where(entity => entity.Id == id);

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> GetByGuidAsync(Guid guid, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().Where(entity => entity.Guid == guid);

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public T Update(T entity)
    {
        entity.LastUpdated = DateTime.Now;

        return _dbSet.Update(entity).Entity;
    }

    public T Delete(T entity) =>
        _dbSet.Remove(entity).Entity;
}