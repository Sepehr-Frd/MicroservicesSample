using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RedditMockup.Common.Dtos;
using RedditMockup.ExternalService.RabbitMQService.Contracts;
using Serilog;
using System.Text;
using System.Text.Json;

namespace RedditMockup.ExternalService.RabbitMQService;

public class MessageBusClient : IMessageBusClient
{
    private readonly ILogger _logger;

    private readonly IConnection _connection;

    private readonly IModel _channel;

    private const string TriggerExchange = "trigger";

    public MessageBusClient(IConfiguration configuration, ILogger logger)
    {
        _logger = logger;

        _logger.Warning("Test form RabbitMQ class");

        var connectionFactory = new ConnectionFactory
        {
            HostName = configuration.GetSection("RabbitMQ").GetValue<string>("RabbitMQHost"),
            Port = configuration.GetSection("RabbitMQ").GetValue<int>("RabbitMQPort")
        };

        try
        {
            _connection = connectionFactory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: TriggerExchange, type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMqConnectionShutdownEventHandler;

            _logger.Information("Connected to the Message Bus");
        }
        catch (Exception exception)
        {
            _logger.Error(exception, "Exception thrown while trying to connect to the Message Bus.");
            throw;
        }
    }

    public void PublishNewQuestion(QuestionPublishedDto questionPublishedDto)
    {
        var message = JsonSerializer.Serialize(questionPublishedDto);

        if (_connection.IsOpen)
        {
            SendMessage(message);
        }
        else
        {
            _logger.Error("Message not sent due to message bus connection being closed.");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: TriggerExchange,
            routingKey: "",
            basicProperties: null,
            body: body);

        _logger.Information("{Message} was sent over {ExchangeName} exchange", message, TriggerExchange);
    }

    private void RabbitMqConnectionShutdownEventHandler(object? sender, ShutdownEventArgs shutdownEventArgs)
    {
        _logger.Information("RabbitMQ connection was shutdown by {Sender}", sender);
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }

        _logger.Information("Message Bus Disposed");
    }
}