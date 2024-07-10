using System.Net;
using Mapster;
using Sieve.Models;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.Business.Base;

public class PublicBaseBusiness<TEntity, TDto> : IPublicBaseBusiness<TDto>
    where TDto : BaseDto
    where TEntity : BaseEntityWithGuid
{
    // [Fields]

    private readonly IBaseBusiness<TEntity, TDto> _baseBusiness;

    protected PublicBaseBusiness(IBaseBusiness<TEntity, TDto> baseBusiness)
    {
        _baseBusiness = baseBusiness;
    }

    public async Task<CustomResponse<TDto>> PublicCreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _baseBusiness.CreateAsync(dto, cancellationToken);

        if (entity is null)
        {
            return CustomResponse<TDto>.CreateUnsuccessfulResponse(HttpStatusCode.InternalServerError);
        }

        var entityDto = entity.Adapt<TDto>();

        return CustomResponse<TDto>.CreateSuccessfulResponse(entityDto, httpStatusCode: HttpStatusCode.Created);
    }

    public async Task<CustomResponse<TDto>> PublicGetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _baseBusiness.GetByGuidAsync(guid, cancellationToken);

        if (entity is null)
        {
            return CustomResponse<TDto>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound);
        }

        var entityDto = entity.Adapt<TDto>();

        return CustomResponse<TDto>.CreateSuccessfulResponse(entityDto);
    }

    public async Task<CustomResponse<List<TDto>>> PublicGetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default)
    {
        var entities = await _baseBusiness.GetAllAsync(sieveModel, cancellationToken);

        var dtos = entities.Adapt<List<TDto>>();

        return CustomResponse<List<TDto>>.CreateSuccessfulResponse(dtos);
    }

    public async Task<CustomResponse<TDto>> PublicUpdateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _baseBusiness.UpdateAsync(dto, cancellationToken);

        if (entity is null)
        {
            return CustomResponse<TDto>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound);
        }

        var updatedDto = entity.Adapt<TDto>();

        return CustomResponse<TDto>.CreateSuccessfulResponse(updatedDto);
    }

    public async Task<CustomResponse<TDto>> PublicDeleteByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        var deletedEntity = await _baseBusiness.DeleteByGuidAsync(guid, cancellationToken);

        if (deletedEntity is null)
        {
            return CustomResponse<TDto>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound);
        }

        var deletedDto = deletedEntity.Adapt<TDto>();

        return CustomResponse<TDto>.CreateSuccessfulResponse(deletedDto);
    }
}