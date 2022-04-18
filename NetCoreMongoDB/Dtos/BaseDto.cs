namespace NetCoreMongoDB.Dtos;

public class BaseDto
{
    public Object? Creator { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}