using ToDoListManager.Common.Dtos;

namespace ToDoListManager.ExternalService.RabbitMQService.Contracts;

public interface IMessageBusClient : IDisposable
{
    void PublishNewToDoItem(ToDoItemPublishedDto toDoItemPublishedDto);
}