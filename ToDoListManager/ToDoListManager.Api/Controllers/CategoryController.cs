using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryBusiness _categoryBusiness;

    public CategoryController(ICategoryBusiness categoryBusiness)
    {
        _categoryBusiness = categoryBusiness;
    }

    [HttpGet]
    [Route("{categoryGuid:guid}")]
    public async Task<ActionResult<CustomResponse<CategoryDto?>>> GetCategoryByGuidAsync([FromRoute] Guid categoryGuid, CancellationToken cancellationToken)
    {
        var result = await _categoryBusiness.GetCategoryByGuidAsync(categoryGuid, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpGet]
    public async Task<ActionResult<CustomResponse<List<CategoryDto>?>>> GetUserCategoriesAsync([FromQuery] PaginationDto? paginationDto, CancellationToken cancellationToken)
    {
        var result = await _categoryBusiness.GetUserCategoriesAsync(paginationDto, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<CustomResponse<CategoryDto?>>> CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        var result = await _categoryBusiness.CreateCategoryAsync(categoryDto, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPut]
    public async Task<ActionResult<CustomResponse<CategoryDto?>>> UpdateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        var result = await _categoryBusiness.UpdateCategoryAsync(categoryDto, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpDelete]
    [Route("{categoryGuid:guid}")]
    public async Task<ActionResult<CustomResponse<CategoryDto?>>> DeleteCategoryByGuidAsync(Guid categoryGuid, CancellationToken cancellationToken)
    {
        var result = await _categoryBusiness.DeleteCategoryByGuidAsync(categoryGuid, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }
}