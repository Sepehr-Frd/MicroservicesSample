using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.Model.Entities;

public class Category : BaseEntityWithGuid
{
    public required string Name { get; init; }

    public required int UserId { get; init; }
    
    public User? User { get; init; }
    
    public ICollection<ToDoItem>? ToDoItems { get; init; }
}