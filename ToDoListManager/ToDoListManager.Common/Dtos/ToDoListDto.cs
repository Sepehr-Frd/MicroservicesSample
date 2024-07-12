namespace ToDoListManager.Common.Dtos;

public record ToDoListDto(Guid Guid, string Name, Guid UserGuid, string? Description = null);