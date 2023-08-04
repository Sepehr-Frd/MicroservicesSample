using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataSelector.Model.Models;

public class BaseDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}