namespace ChangeDataCaptureHub.Model.Models;

public class MongoDbSettings
{
    public string ConnectionString { get; init; } = null!;

    public string DatabaseName { get; init; } = null!;

    public string CollectionName { get; init; } = null!;
}