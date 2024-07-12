using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.DataAccess.Contracts;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken);

    Task<List<T>> GetAllAsync(
        PaginationDto? paginationDto = null,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default);

    Task<T?> GetByGuidAsync(Guid guid, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default);

    T Update(T entity);

    T Delete(T entity);
}