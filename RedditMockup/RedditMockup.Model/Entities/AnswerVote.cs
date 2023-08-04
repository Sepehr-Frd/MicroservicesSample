using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class AnswerVote : BaseEntity
{
    #region [Properties]

    [Sieve(CanSort = true)]
    public bool Kind { get; set; }

    #endregion

    #region [Navigation Properties]

    public int AnswerId { get; set; }
    public Answer? Answer { get; set; }

    #endregion
}