using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChangeDataCaptureHub.Model.Models;

public class BaseDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}