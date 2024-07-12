namespace ChangeDataCaptureHub.ExternalService.RabbitMQ.EventProcessing;

public interface IEventProcessor
{
    Task ProcessEventAsync(string message, CancellationToken cancellationToken);
}