using RedditMockup.Common.Dtos;
using Sieve.Models;

namespace RedditMockup.Business.Contracts;

public interface IPublicBaseBusiness<TDto>
{
    Task<CustomResponse<TDto>> PublicCreateAsync(TDto dto, CancellationToken cancellationToken = default);

    Task<CustomResponse<TDto>> PublicGetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<CustomResponse<List<TDto>>> PublicGetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default);

    Task<CustomResponse<TDto>> PublicUpdateAsync(TDto dto, CancellationToken cancellationToken = default);

    Task<CustomResponse<TDto>> PublicDeleteByGuidAsync(Guid guid, CancellationToken cancellationToken = default);
}