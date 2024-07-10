using Microsoft.EntityFrameworkCore.Query;
using Sieve.Models;
using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.DataAccess.Contracts;

public interface IBaseRepository<T> where T : BaseEntityWithGuid
{
    Task<T> CreateAsync(T t, CancellationToken cancellationToken);

    Task<List<T>> GetAllAsync(SieveModel sieveModel, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default);

    Task<T?> GetByGuidAsync(Guid guid, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default);

    T Update(T t);

    T Delete(T t);
}