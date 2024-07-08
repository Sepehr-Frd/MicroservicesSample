namespace RedditMockup.Common.Dtos;

public class AnswerDto : BaseDto
{
    public Guid QuestionGuid { get; init; }

    public Guid UserGuid { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }
}