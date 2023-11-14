using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RedditMockup.DataAccess.Context;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.Model.BaseEntities;
using Sieve.Models;
using Sieve.Services;

namespace RedditMockup.DataAccess.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntityWithGuid
{
    // [Fields]

    private readonly DbSet<T> _dbSet;

    private readonly ISieveProcessor _processor;

    

    // [Constructor]

    protected BaseRepository(RedditMockupContext context, ISieveProcessor processor)
    {
        _processor = processor;
        _dbSet = context.Set<T>();
    }

    

    // [Methods]

    public async Task<T> CreateAsync(T t, CancellationToken cancellationToken = default)
    {
        var createdEntity = (await _dbSet.AddAsync(t, cancellationToken)).Entity;

        return createdEntity;
    }

    public async Task<List<T>> GetAllAsync(SieveModel sieveModel, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking();
        if (include != null)
            query = include(query);

        return await _processor.Apply(sieveModel, query).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().Where(t => t.Id == id);

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> GetByGuidAsync(Guid guid, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().Where(t => t.Guid == guid);

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public T Update(T t)
    {
        t.LastUpdated = DateTime.Now;

        return _dbSet.Update(t).Entity;
    }

    public T Delete(T t) =>
        _dbSet.Remove(t).Entity;

    
}