namespace ToDoListManager.Model.BaseEntities;

public class BaseEntityWithGuid : BaseEntity
{
    protected BaseEntityWithGuid() => Guid = Guid.NewGuid();

    public Guid Guid { get; }

}