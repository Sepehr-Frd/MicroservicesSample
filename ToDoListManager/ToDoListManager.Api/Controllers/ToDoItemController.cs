using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/to-do-items")]
public class ToDoItemController : ControllerBase
{
    private readonly IToDoItemBusiness _toDoItemBusiness;

    public ToDoItemController(IToDoItemBusiness toDoItemBusiness)
    {
        _toDoItemBusiness = toDoItemBusiness;
    }

    [HttpGet]
    [Route("{toDoItemGuid:guid}")]
    public async Task<ActionResult<CustomResponse<ToDoItemDto?>>> GetToDoItemByGuidAsync([FromRoute] Guid toDoItemGuid, CancellationToken cancellationToken)
    {
        var result = await _toDoItemBusiness.GetToDoItemByGuidAsync(toDoItemGuid, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpGet]
    public async Task<ActionResult<CustomResponse<List<ToDoItemDto>?>>> GetToDoItemsByListIdAsync(
        [FromQuery] Guid toDoListGuid,
        [FromQuery] PaginationDto? paginationDto,
        CancellationToken cancellationToken)
    {
        var result = await _toDoItemBusiness.GetToDoItemsByListGuidAsync(toDoListGuid, paginationDto, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<CustomResponse<ToDoItemDto?>>> CreateToDoItemAsync(ToDoItemDto toDoItemDto, CancellationToken cancellationToken)
    {
        var result = await _toDoItemBusiness.CreateToDoItemAsync(toDoItemDto, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPut]
    public async Task<ActionResult<CustomResponse<ToDoItemDto?>>> UpdateToDoItemAsync(ToDoItemDto toDoItemDto, CancellationToken cancellationToken)
    {
        var result = await _toDoItemBusiness.UpdateToDoItemAsync(toDoItemDto, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpDelete("{toDoItemGuid:guid}")]
    public async Task<ActionResult<CustomResponse<ToDoItemDto?>>> DeleteToDoItemByGuidAsync(Guid toDoItemGuid, CancellationToken cancellationToken)
    {
        var result = await _toDoItemBusiness.DeleteToDoItemByGuidAsync(toDoItemGuid, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }
}