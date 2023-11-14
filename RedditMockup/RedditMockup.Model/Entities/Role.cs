using RedditMockup.Model.BaseEntities;

namespace RedditMockup.Model.Entities;

public class Role : BaseEntityWithGuid
{
    // [Properties]

    public string? Title { get; set; }

    

    // [Navigation Properties]

    public ICollection<UserRole>? UserRoles { get; set; }

    
}