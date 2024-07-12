namespace ChangeDataCaptureHub.Common.Enums;

public enum GrpcEvent : byte
{
    EntityCreated = 0,
    EntityUpdated = 1,
    EntityRemoved = 2
}