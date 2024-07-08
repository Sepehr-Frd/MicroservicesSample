using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class UserRole : BaseEntity
{
    // [Navigation Properties]

    [Sieve(CanFilter = true, CanSort = true)]
    public int UserId { get; init; }

    public User? User { get; init; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int RoleId { get; init; }

    public Role? Role { get; init; }

}