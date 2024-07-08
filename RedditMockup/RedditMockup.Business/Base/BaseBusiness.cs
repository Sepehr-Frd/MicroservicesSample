using Mapster;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.Model.BaseEntities;
using Sieve.Models;

namespace RedditMockup.Business.Base;

public abstract class BaseBusiness<TEntity, TDto> : IBaseBusiness<TEntity, TDto>
    where TEntity : BaseEntityWithGuid
    where TDto : BaseDto
{
    // [Fields]

    private readonly IUnitOfWork _unitOfWork;

    private readonly IBaseRepository<TEntity> _repository;

    // [Constructor]

    protected BaseBusiness(IUnitOfWork unitOfWork, IBaseRepository<TEntity> repository)
    {
        _unitOfWork = unitOfWork;

        _repository = repository;
    }

    // [Methods]

    public abstract Task<TEntity?> CreateAsync(TDto userDto, CancellationToken cancellationToken = default);

    protected async Task<TEntity?> CreateAsync(TEntity t, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(t, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return createdEntity;
    }

    public abstract Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public abstract Task<TEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    public abstract Task<List<TEntity>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default);

    public async Task<TEntity?> UpdateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        var t = await GetByGuidAsync(dto.Guid, cancellationToken);

        if (t is null)
        {
            return null;
        }

        dto.Adapt(t);

        var updatedEntity = _repository.Update(t);

        await _unitOfWork.CommitAsync(cancellationToken);

        return updatedEntity;
    }

    public async Task<TEntity?> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, null, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var deletedEntity = _repository.Delete(entity);

        await _unitOfWork.CommitAsync(cancellationToken);

        return deletedEntity;
    }

    public async Task<TEntity?> DeleteByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await GetByGuidAsync(guid, cancellationToken);

        return entity is null ? null : _repository.Delete(entity);
    }

}