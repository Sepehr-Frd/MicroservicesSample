using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Business.Contracts;

public interface ICategoryBusiness
{
    Task<CustomResponse<CategoryDto?>> GetCategoryByGuidAsync(Guid categoryGuid, CancellationToken cancellationToken = default);

    Task<CustomResponse<List<CategoryDto>?>> GetUserCategoriesAsync(PaginationDto? paginationDto = null, CancellationToken cancellationToken = default);

    Task<CustomResponse<CategoryDto?>> CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default);

    Task<CustomResponse<CategoryDto?>> UpdateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default);

    Task<CustomResponse<CategoryDto?>> DeleteCategoryByGuidAsync(Guid categoryGuid, CancellationToken cancellationToken = default);
}