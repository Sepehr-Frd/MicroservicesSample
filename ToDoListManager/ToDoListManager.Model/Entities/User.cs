using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.Model.Entities;

public class User : BaseEntity
{
    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string PasswordHash { get; init; }

    public required long PersonId { get; set; }

    public Person? Person { get; init; }

    public ICollection<ToDoList> ToDoLists { get; init; } = [];

    public ICollection<Category> Categories { get; init; } = [];
}