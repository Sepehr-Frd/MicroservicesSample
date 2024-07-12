using ToDoListManager.Model.BaseEntities;
using ToDoListManager.Model.Enums;

namespace ToDoListManager.Model.Entities;

public class ToDoItem : BaseEntity
{
    public required string Title { get; init; }

    public string? Description { get; init; }

    public bool IsCompleted { get; init; }

    public DateTime DueDate { get; init; }

    public Priority Priority { get; init; }

    public required long ToDoListId { get; set; }

    public ToDoList? ToDoList { get; init; }

    public long? CategoryId { get; set; }

    public Category? Category { get; init; }
}