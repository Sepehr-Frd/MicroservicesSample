using AutoMapper;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.BaseEntities;
using Sieve.Models;
using System.Net;

namespace RedditMockup.Business.Base;

public class PublicBaseBusiness<TEntity, TDto> : IPublicBaseBusiness<TDto>
    where TDto : BaseDto
    where TEntity : BaseEntityWithGuid
{
    // [Fields]

    private readonly IMapper _mapper;

    private readonly IBaseBusiness<TEntity, TDto> _baseBusiness;

    

    protected PublicBaseBusiness(IBaseBusiness<TEntity, TDto> baseBusiness, IMapper mapper)
    {
        _baseBusiness = baseBusiness;

        _mapper = mapper;
    }

    public async Task<CustomResponse<TDto>> PublicCreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _baseBusiness.CreateAsync(dto, cancellationToken);

        if (entity is null)
        {
            return CustomResponse<TDto>.CreateUnsuccessfulResponse(HttpStatusCode.InternalServerError);
        }

        var entityDto = _mapper.Map<TDto>(entity);

        return CustomResponse<TDto>.CreateSuccessfulResponse(entityDto, httpStatusCode: HttpStatusCode.Created);
    }

    public async Task<CustomResponse<TDto>> PublicGetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _baseBusiness.GetByGuidAsync(guid, cancellationToken);

        if (entity is null)
        {
            return CustomResponse<TDto>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound);
        }

        var entityDto = _mapper.Map<TDto>(entity);

        return CustomResponse<TDto>.CreateSuccessfulResponse(entityDto);
    }

    public async Task<CustomResponse<List<TDto>>> PublicGetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default)
    {
        var entities = await _baseBusiness.GetAllAsync(sieveModel, cancellationToken);

        var dtos = _mapper.Map<List<TDto>>(entities);

        return CustomResponse<List<TDto>>.CreateSuccessfulResponse(dtos);
    }

    public async Task<CustomResponse<TDto>> PublicUpdateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _baseBusiness.UpdateAsync(dto, cancellationToken);

        if (entity is null)
        {
            return CustomResponse<TDto>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound);
        }

        var updatedDto = _mapper.Map<TDto>(entity);

        return CustomResponse<TDto>.CreateSuccessfulResponse(updatedDto);
    }

    public async Task<CustomResponse<TDto>> PublicDeleteByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        TEntity? deletedEntity = await _baseBusiness.DeleteByGuidAsync(guid, cancellationToken);

        if (deletedEntity is null)
        {
            return CustomResponse<TDto>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound);
        }

        var deletedDto = _mapper.Map<TDto>(deletedEntity);

        return CustomResponse<TDto>.CreateSuccessfulResponse(deletedDto);
    }
}