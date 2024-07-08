using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class QuestionVote : BaseEntity
{
    // [Properties]

    [Sieve(CanSort = true)]
    public bool Kind { get; init; }

    // [Navigation Properties]

    public int QuestionId { get; init; }

    public Question? Question { get; init; }

}