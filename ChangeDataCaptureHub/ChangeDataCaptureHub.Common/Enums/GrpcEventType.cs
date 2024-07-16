namespace ChangeDataCaptureHub.Common.Enums;

public enum GrpcEventType : byte
{
    EntityCreated = 0,
    EntityUpdated = 1,
    EntityRemoved = 2
}