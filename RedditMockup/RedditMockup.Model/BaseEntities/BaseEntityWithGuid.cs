using Microsoft.EntityFrameworkCore;

namespace RedditMockup.Model.BaseEntities;

[Index("Guid", IsUnique = true)]
public class BaseEntityWithGuid : BaseEntity
{
    // [Constructor]

    protected BaseEntityWithGuid() => Guid = Guid.NewGuid();

    

    // [Properties]

    public Guid Guid { get; }

    
}