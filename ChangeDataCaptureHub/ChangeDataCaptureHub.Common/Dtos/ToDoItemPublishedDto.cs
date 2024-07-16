using ChangeDataCaptureHub.Common.Enums;

namespace ChangeDataCaptureHub.Common.Dtos;

public class ToDoItemPublishedDto
{
    public required ToDoItemDto ToDoItemDto { get; init; }

    public GrpcEventType EventType { get; init; }
}