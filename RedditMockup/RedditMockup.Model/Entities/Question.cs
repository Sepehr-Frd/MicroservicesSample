using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Question : BaseEntityWithGuid
{
    #region [Properties]

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Title { get; set; }

    [Sieve(CanFilter = true)]
    public string? Description { get; set; }

    #endregion

    #region [Navigation Properties]

    public ICollection<Answer>? Answers { get; set; }

    public ICollection<QuestionVote>? Votes { get; set; }

    public ICollection<Bookmark>? Bookmarks { get; set; }
    
    [Sieve(CanFilter = true)]
    public int UserId { get; set; }
    
    public User? User { get; set; }

    #endregion
}