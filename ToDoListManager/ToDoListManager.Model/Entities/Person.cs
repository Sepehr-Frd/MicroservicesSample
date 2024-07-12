using ToDoListManager.Model.BaseEntities;

namespace ToDoListManager.Model.Entities;

public class Person : BaseEntity
{
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string FullName => FirstName + " " + LastName;

    public User? User { get; init; }
}