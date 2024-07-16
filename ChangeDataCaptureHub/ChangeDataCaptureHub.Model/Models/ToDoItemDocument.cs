using ChangeDataCaptureHub.Model.Enums;

namespace ChangeDataCaptureHub.Model.Models;

public class ToDoItemDocument : BaseDocument
{
    public required string Title { get; init; }

    public string? Description { get; init; }

    public bool IsCompleted { get; init; }

    public DateTime DueDate { get; init; }

    public Priority Priority { get; init; }

    public Guid ToDoListGuid { get; init; }

    public Guid? CategoryGuid { get; init; }
}