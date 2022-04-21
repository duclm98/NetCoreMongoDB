namespace NetCoreMongoDB.Dtos.Authentication;

public class RegisterCreateDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
}