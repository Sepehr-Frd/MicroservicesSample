using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;
using ToDoListManager.Common.Dtos;
using ToDoListManager.ExternalService.RabbitMQService.Contracts;

namespace ToDoListManager.ExternalService.RabbitMQService;

public sealed class MessageBusClient : IMessageBusClient
{
    private readonly ILogger _logger;
    private readonly IConnection _connection;
    private readonly RabbitMqConfigurationDto _rabbitMqConfigurationDto;

    public MessageBusClient(IConnection connection, ILogger logger, IConfiguration configuration)
    {
        _logger = logger;
        _connection = connection;
        _rabbitMqConfigurationDto = configuration.GetSection("RabbitMqConfiguration").Get<RabbitMqConfigurationDto>()!;
    }

    public async Task PublishNewToDoItemAsync(ToDoItemPublishedDto toDoItemPublishedDto)
    {
        if (_connection.IsOpen)
        {
            await using var channel = await _connection.CreateChannelAsync();

            var message = JsonSerializer.Serialize(toDoItemPublishedDto);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                _rabbitMqConfigurationDto.TriggerExchangeName,
                _rabbitMqConfigurationDto.TriggerQueueName,
                body);

            _logger.Information("{0} was sent over {1} exchange", message, _rabbitMqConfigurationDto.TriggerExchangeName);
        }
        else
        {
            _logger.Error("Message not sent due to message bus connection being closed.");
        }
    }
}