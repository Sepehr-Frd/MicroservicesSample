using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class AnswerVote : BaseEntity
{
    // [Properties]

    [Sieve(CanSort = true)]
    public bool Kind { get; init; }

    // [Navigation Properties]

    public int AnswerId { get; init; }

    public Answer? Answer { get; init; }

}