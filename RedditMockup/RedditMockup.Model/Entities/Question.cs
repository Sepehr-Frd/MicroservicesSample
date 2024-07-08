using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Question : BaseEntityWithGuid
{
    // [Properties]

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Title { get; init; }

    [Sieve(CanFilter = true)]
    public string? Description { get; init; }

    // [Navigation Properties]

    public ICollection<Answer>? Answers { get; init; }

    public ICollection<QuestionVote>? Votes { get; init; }

    public ICollection<Bookmark>? Bookmarks { get; init; }

    [Sieve(CanFilter = true)]
    public int UserId { get; set; }

    public User? User { get; init; }

}