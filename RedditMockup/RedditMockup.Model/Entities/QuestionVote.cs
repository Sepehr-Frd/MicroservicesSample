using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class QuestionVote : BaseEntity
{
    #region [Properties]

    [Sieve(CanSort = true)]
    public bool Kind { get; set; }

    #endregion

    #region [Navigation Properties]

    public int QuestionId { get; set; }

    public Question? Question { get; set; }

    #endregion
}