using ChangeDataCaptureHub.Common.Enums;

namespace ChangeDataCaptureHub.Common.Dtos;

public class ToDoItemPublishedDto
{
    public required ToDoItemDto ToDoItemDto { get; init; }

    public GrpcEvent Event { get; init; }
}