using ChangeDataCaptureHub.Model.Enums;

namespace ChangeDataCaptureHub.Common.Dtos;

public record ToDoItemDto(
    Guid Guid,
    string Title,
    bool IsCompleted,
    DateTime DueDate,
    Priority Priority,
    Guid ToDoListGuid,
    Guid? CategoryGuid = null,
    string? Description = null);