using ChangeDataCaptureHub.Common.Dtos;

namespace ChangeDataCaptureHub.ExternalService.ToDoListManager.ToDoListManagerGrpcService;

public interface IToDoListManagerDataClient
{
    IEnumerable<ToDoItemDto>? ReturnAllToDoItems();
}