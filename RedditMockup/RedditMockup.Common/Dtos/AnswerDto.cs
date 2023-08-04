namespace RedditMockup.Common.Dtos;

public class AnswerDto : BaseDto
{
    public Guid QuestionGuid { get; set; }

    public Guid UserGuid { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
}