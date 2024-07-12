using ToDoListManager.Model.Entities;

namespace ToDoListManager.Common.Dtos;

public record UserDto(
    Guid Guid,
    string Username,
    string Email,
    ICollection<ToDoListDto> ToDoLists,
    ICollection<Category> Categories,
    string FirstName,
    string? LastName = null);