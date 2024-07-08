using Sieve.Models;

namespace RedditMockup.Business.Contracts;

public interface IBaseBusiness<TEntity, in TDto>
{
    Task<TEntity?> CreateAsync(TDto userDto, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<List<TEntity>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default);

    Task<TEntity?> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TEntity?> DeleteByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<TEntity?> UpdateAsync(TDto dto, CancellationToken cancellationToken = default);

}