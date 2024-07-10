using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.Model.Entities;

public class User : BaseEntityWithGuid
{
    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string PasswordHash { get; init; }

    public required int PersonId { get; set; }

    public Person? Person { get; init; }

    public ICollection<ToDoList>? ToDoLists { get; init; }

    public ICollection<Category>? Categories { get; init; }
}