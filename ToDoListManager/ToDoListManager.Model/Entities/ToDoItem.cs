using ToDoListManager.Model.BaseEntities;
using ToDoListManager.Model.Enums;

namespace ToDoListManager.Model.Entities;

public class ToDoItem : BaseEntityWithGuid
{
    public required string Title { get; init; }

    public string? Description { get; init; }

    public bool IsCompleted { get; init; }

    public DateTime DueDate { get; init; }

    public Priority Priority { get; init; }

    public int ToDoListId { get; init; }

    public ToDoList? ToDoList { get; init; }

    public int CategoryId { get; init; }

    public Category? Category { get; init; }
}