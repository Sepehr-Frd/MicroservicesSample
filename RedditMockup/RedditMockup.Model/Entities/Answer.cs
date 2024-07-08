using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Answer : BaseEntityWithGuid
{
    // [Properties]

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Title { get; init; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Description { get; init; }

    // [Navigation Properties]

    public ICollection<AnswerVote>? Votes { get; init; }

    [Sieve(CanFilter = true)]
    public int QuestionId { get; set; }

    public Question? Question { get; init; }

    [Sieve(CanFilter = true)]
    public int UserId { get; set; }

    public User? User { get; init; }

}