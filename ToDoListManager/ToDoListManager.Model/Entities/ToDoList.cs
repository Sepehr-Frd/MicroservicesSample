using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.Model.Entities;

public class ToDoList : BaseEntity
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public required long UserId { get; set; }

    public User? User { get; init; }

    public ICollection<ToDoItem> ToDoItems { get; init; } = [];
}