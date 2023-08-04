using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Answer : BaseEntityWithGuid
{
    #region [Properties]

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Title { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Description { get; set; }

    #endregion

    #region [Navigation Properties]

    public ICollection<AnswerVote>? Votes { get; set; }

    [Sieve(CanFilter = true)]
    public int QuestionId { get; set; }

    public Question? Question { get; set; }

    [Sieve(CanFilter = true)]
    public int UserId { get; set; }

    public User? User { get; set; }

    #endregion
}