using Microsoft.EntityFrameworkCore;

namespace RedditMockup.Model.BaseEntities;

[Index("Guid", IsUnique = true)]
public class BaseEntityWithGuid : BaseEntity
{
    #region [Constructor]

    protected BaseEntityWithGuid() => Guid = Guid.NewGuid();

    #endregion

    #region [Properties]

    public Guid Guid { get; }

    #endregion
}