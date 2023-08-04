namespace RedditMockup.Common.Dtos;

public class QuestionDto : BaseDto
{
    public Guid UserGuid { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}