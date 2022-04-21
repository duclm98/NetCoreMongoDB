namespace NetCoreMongoDB.Dtos.Book;

public class BookCreateDto
{
    public string BookName { get; set; }
    public int Price { get; set; }
    public List<string> CategoryIds { get; set; } = new();
    public string Author { get; set; }
}