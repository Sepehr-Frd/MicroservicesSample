using ToDoListManager.Model.Enums;

namespace ToDoListManager.Common.Dtos;

public record ToDoItemDto
{
    public ToDoItemDto(
        Guid guid,
        string title,
        bool isCompleted,
        DateTime dueDate,
        Priority priority,
        Guid toDoListGuid,
        Guid? categoryGuid = null,
        string? description = null)
    {
        Guid = guid;
        Title = title;
        IsCompleted = isCompleted;
        DueDate = dueDate;
        Priority = priority;
        ToDoListGuid = toDoListGuid;
        CategoryGuid = categoryGuid;
        Description = description;
    }

    public Guid Guid { get; set; }

    public string Title { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime DueDate { get; set; }

    public Priority Priority { get; set; }

    public Guid ToDoListGuid { get; set; }

    public Guid? CategoryGuid { get; set; }

    public string? Description { get; set; }
}