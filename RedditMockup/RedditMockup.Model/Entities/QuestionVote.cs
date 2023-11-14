using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class QuestionVote : BaseEntity
{
    // [Properties]

    [Sieve(CanSort = true)]
    public bool Kind { get; set; }

    

    // [Navigation Properties]

    public int QuestionId { get; set; }

    public Question? Question { get; set; }

    
}