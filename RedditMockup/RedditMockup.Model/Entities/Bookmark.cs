using RedditMockup.Model.BaseEntities;

namespace RedditMockup.Model.Entities;

public class Bookmark : BaseEntity
{
    // [Properties]
    public bool IsBookmarked { get; init; }

    // [Navigation Properties]

    public int UserId { get; init; }

    public User? User { get; init; }

    public int QuestionId { get; init; }

    public Question? Question { get; init; }

}