using ToDoListManager.Common.Dtos;

namespace ToDoListManager.ExternalService.RabbitMQService.Contracts;

public interface IMessageBusClient
{
    Task PublishNewToDoItemAsync(ToDoItemPublishedDto toDoItemPublishedDto);
}