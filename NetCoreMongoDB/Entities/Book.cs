using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NetCoreMongoDB.Entities;

public class Book : Base
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string BookId { get; set; }

    public string BookName { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public string Author { get; set; }
}