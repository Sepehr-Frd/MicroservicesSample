using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.Model.Entities;

public class ToDoList : BaseEntityWithGuid
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public int UserId { get; init; }

    public User? User { get; init; }

    public ICollection<ToDoItem>? ToDoItems { get; init; }
}