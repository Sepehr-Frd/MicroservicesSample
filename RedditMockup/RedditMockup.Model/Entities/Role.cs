using RedditMockup.Model.BaseEntities;

namespace RedditMockup.Model.Entities;

public class Role : BaseEntityWithGuid
{
    // [Properties]

    public string? Title { get; init; }

    // [Navigation Properties]

    public ICollection<UserRole>? UserRoles { get; init; }

}