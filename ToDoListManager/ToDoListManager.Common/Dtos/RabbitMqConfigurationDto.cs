namespace ToDoListManager.Common.Dtos;

public class RabbitMqConfigurationDto
{
    public required string Host { get; set; }

    public required int Port { get; set; }

    public required string TriggerExchangeName { get; set; }

    public required string TriggerQueueName { get; set; }
}