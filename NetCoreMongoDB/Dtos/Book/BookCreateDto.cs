namespace NetCoreMongoDB.Dtos.Book;

public class BookCreateDto
{
    public string BookName { get; set; }
    public int Price { get; set; }
    public string CategoryId { get; set; }
    public string Author { get; set; }
}