using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class UserRole : BaseEntity
{
    // [Navigation Properties]

    [Sieve(CanFilter = true, CanSort = true)]
    public int UserId { get; set; }

    public User? User { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int RoleId { get; set; }

    public Role? Role { get; set; }

    
}