using AutoMapper;
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

    private readonly IMapper _mapper;

    

    // [Constructor]

    protected BaseBusiness(IUnitOfWork unitOfWork, IBaseRepository<TEntity> repository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;

        _repository = repository;

        _mapper = mapper;
    }

    

    // [Methods]

    public abstract Task<TEntity?> CreateAsync(TDto answerDto, CancellationToken cancellationToken = default);

    protected async Task<TEntity?> CreateAsync(TEntity t, CancellationToken cancellationToken = default)
    {
        TEntity createdEntity = await _repository.CreateAsync(t, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return createdEntity;
    }
    
    public abstract Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public abstract Task<TEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    public abstract Task<List<TEntity>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default);

    public async Task<TEntity?> UpdateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        TEntity? t = await GetByGuidAsync(dto.Guid, cancellationToken);

        if (t is null)
        {
            return null;
        }

        _mapper.Map(dto, t);

        TEntity updatedEntity = _repository.Update(t);

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

        TEntity deletedEntity = _repository.Delete(entity);

        await _unitOfWork.CommitAsync(cancellationToken);

        return deletedEntity;
    }

    public async Task<TEntity?> DeleteByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await GetByGuidAsync(guid, cancellationToken);

        return entity is null ? null : _repository.Delete(entity);
    }

    
}