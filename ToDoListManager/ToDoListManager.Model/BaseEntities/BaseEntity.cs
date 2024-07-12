namespace ToDoListManager.Model.BaseEntities;

public class BaseEntity
{
    protected BaseEntity()
    {
        CreationDate = LastUpdated = DateTime.Now;
        Guid = Guid.NewGuid();
    }

    public long Id { get; init; }

    public DateTime CreationDate { get; init; }

    public DateTime LastUpdated { get; set; }

    public Guid Guid { get; init; }
}