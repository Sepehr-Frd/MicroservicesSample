using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class User : BaseEntityWithGuid
{
    // [Properties]

    [Sieve(CanSort = true, CanFilter = true)]
    public string? Username { get; init; }

    public string? Password { get; init; }

    public int Score { get; set; }

    // [Navigation Properties]

    [Sieve(CanFilter = true)]
    public int PersonId { get; set; }

    public Person? Person { get; init; }

    public Profile? Profile { get; init; }

    public ICollection<Question>? Questions { get; init; }

    public ICollection<Answer>? Answers { get; init; }

    public ICollection<UserRole>? UserRoles { get; init; }

    public ICollection<Bookmark>? Bookmarks { get; init; }
}