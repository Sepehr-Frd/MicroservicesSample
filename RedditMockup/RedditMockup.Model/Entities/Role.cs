using RedditMockup.Model.BaseEntities;

namespace RedditMockup.Model.Entities;

public class Role : BaseEntityWithGuid
{
    #region [Properties]

    public string? Title { get; set; }

    #endregion

    #region [Navigation Properties]

    public ICollection<UserRole>? UserRoles { get; set; }

    #endregion
}