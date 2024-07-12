using ChangeDataCaptureHub.Business.Businesses;
using ChangeDataCaptureHub.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChangeDataCaptureHub.Api.Controllers;

[ApiController]
[Route("api")]
public class BaseController<T> : ControllerBase
    where T : BaseDocument
{
    private readonly BaseBusiness<T> _business;

    public BaseController(BaseBusiness<T> baseBusiness) =>
        _business = baseBusiness;

    [HttpPost]
    [Route("single-creation")]
    public async Task CreateOneAsync([FromBody] T t, CancellationToken cancellationToken) =>
        await _business.CreateOneAsync(t, cancellationToken);

    [HttpPost]
    [Route("multi-creation")]
    public async Task CreateManyAsync([FromBody] List<T> values, CancellationToken cancellationToken) =>
        await _business.CreateManyAsync(values, cancellationToken);

    [HttpGet]
    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken) =>
        await _business.GetAllAsync(cancellationToken);

    [HttpGet]
    [Route("id/{id}")]
    public async Task<T?> GetByIdAsync([FromQuery] string id, CancellationToken cancellationToken) =>
        await _business.GetByIdAsync(id, cancellationToken);

    [HttpPut]
    public async Task<bool> UpdateOneAsync([FromBody] T t, CancellationToken cancellationToken) =>
        await _business.UpdateOneAsync(t, cancellationToken);

    [HttpDelete]
    public async Task<bool> DeleteByIdAsync([FromQuery] string id, CancellationToken cancellationToken) =>
        await _business.DeleteByIdAsync(id, cancellationToken);
}