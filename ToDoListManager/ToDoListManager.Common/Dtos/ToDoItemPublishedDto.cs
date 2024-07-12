using ToDoListManager.Common.Enums;

namespace ToDoListManager.Common.Dtos;

public class ToDoItemPublishedDto
{
    public required ToDoItemDto ToDoItemDto { get; set; }

    public GrpcEvent Event { get; set; }
}