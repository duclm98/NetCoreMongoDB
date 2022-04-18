using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetCoreMongoDB.Entities;

public class User : Base
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }

    public UserRole Role { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public enum UserRole
{
    [Display(Name = "Quản trị viên")] Admin = 1,
    [Display(Name = "Khách hàng")] Customer = 2
}