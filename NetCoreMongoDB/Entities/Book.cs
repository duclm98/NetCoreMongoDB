using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NetCoreMongoDB.Entities;

public class Book : Base
{
    public string BookName { get; set; }
    public int Price { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; }
    public string Author { get; set; }
}