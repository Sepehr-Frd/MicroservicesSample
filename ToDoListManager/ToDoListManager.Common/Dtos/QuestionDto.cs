namespace ToDoListManager.Common.Dtos;

public class QuestionDto : BaseDto
{
    public Guid UserGuid { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }
}