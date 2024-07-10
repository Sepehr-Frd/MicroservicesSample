namespace ToDoListManager.Model.BaseEntities;

public class BaseEntity
{
    protected BaseEntity() => CreationDate = LastUpdated = DateTime.Now;

    public int Id { get; init; }
    
    public DateTime CreationDate { get; }
    
    public DateTime LastUpdated { get; set; }
}