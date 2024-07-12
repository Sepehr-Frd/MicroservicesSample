using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.Contracts;

public interface IToDoItemBusiness
{
    Task<CustomResponse<ToDoItemDto?>> GetToDoItemByGuidAsync(Guid toDoItemGuid, CancellationToken cancellationToken = default);

    Task<CustomResponse<List<ToDoItemDto>?>> GetToDoItemsByListGuidAsync(Guid toDoListGuid, PaginationDto? paginationDto = null, CancellationToken cancellationToken = default);

    Task<CustomResponse<ToDoItemDto?>> CreateToDoItemAsync(ToDoItemDto toDoItemDto, CancellationToken cancellationToken = default);

    Task<CustomResponse<ToDoItemDto?>> UpdateToDoItemAsync(ToDoItemDto toDoItemDto, CancellationToken cancellationToken = default);

    Task<CustomResponse<ToDoItemDto?>> DeleteToDoItemByGuidAsync(Guid toDoItemGuid, CancellationToken cancellationToken = default);

    Task<List<ToDoItem>> GetAllToDoItemsWithoutPaginationAsync(CancellationToken cancellationToken = default);
}