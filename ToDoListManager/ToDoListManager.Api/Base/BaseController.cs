using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Constants;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.Api.Base;

[Authorize(PolicyConstants.Admin)]
[ApiController]
[Route("api")]
public class BaseController<TEntity, TDto> : ControllerBase
    where TEntity : BaseEntity
    where TDto : BaseDto
{
    private readonly IBaseBusiness<TEntity, TDto> _business;

    public BaseController(IBaseBusiness<TEntity, TDto> business) =>
        _business = business;

    [HttpPost]
    public async Task<TEntity?> CreateAsync([FromBody] TDto dto, CancellationToken cancellationToken) =>
        await _business.CreateAsync(dto, cancellationToken);

    [HttpGet]
    public async Task<List<TEntity>?> GetAllAsync([FromQuery] SieveModel sieveModel, CancellationToken cancellationToken) =>
        await _business.GetAllAsync(sieveModel, cancellationToken);

    [HttpGet]
    [Route("id/{id:int}")]
    public async Task<TEntity?> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken) =>
        await _business.GetByIdAsync(id, cancellationToken);

    [HttpGet]
    [Route("guid/{guid:guid}")]
    public async Task<TEntity?> GetByGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.GetByGuidAsync(guid, cancellationToken);

    [HttpDelete]
    [Route("id/{id:int}")]
    public async Task<TEntity?> DeleteByIdAsync([FromRoute] int id, CancellationToken cancellationToken) =>
        await _business.DeleteByIdAsync(id, cancellationToken);

    [HttpDelete]
    [Route("guid/{guid:guid}")]
    public async Task<TEntity?> DeleteByGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.DeleteByGuidAsync(guid, cancellationToken);

    [HttpPut]
    public async Task<TEntity?> UpdateAsync([FromBody] TDto dto, CancellationToken
        cancellationToken) =>
        await _business.UpdateAsync(dto, cancellationToken);

    [HttpOptions]
    public void Options() =>
        Response.Headers.Append("Allow", "POST,PUT,DELETE,GET");
}