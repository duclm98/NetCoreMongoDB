namespace NetCoreMongoDB.Dtos.Book;

public class BookCreateDto
{
    public string BookName { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public string Author { get; set; } = null!;
}