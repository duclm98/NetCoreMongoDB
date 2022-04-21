using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NetCoreMongoDB.Models;

namespace NetCoreMongoDB.Entities;

public class Base
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public Creator Creator { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
}