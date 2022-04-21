using NetCoreMongoDB.Models;

namespace NetCoreMongoDB.Dtos;

public class CreatorDto
{
    public string CreatorId { get; set; }
    public string CreatorName { get; set; }
    public EnumValue CreatorRole { get; set; }
}