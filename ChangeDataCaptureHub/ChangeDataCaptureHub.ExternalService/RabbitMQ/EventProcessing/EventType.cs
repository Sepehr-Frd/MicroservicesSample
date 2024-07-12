namespace ChangeDataCaptureHub.ExternalService.RabbitMQ.EventProcessing;

internal enum EventType : byte
{
    ToDoItemPublished = 0,
    Undetermined = 1
}