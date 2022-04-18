using MongoDB.Bson;

namespace NetCoreMongoDB.Entities;

public class Base
{
    public BsonDocument Creator { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
}