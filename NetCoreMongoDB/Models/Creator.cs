using NetCoreMongoDB.Entities;

namespace NetCoreMongoDB.Models;

public class Creator
{
    public string CreatorId { get; set; }
    public string CreatorName { get; set; }
    public UserRole CreatorRole { get; set; }
}