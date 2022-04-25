using NetCoreMongoDB.Dtos.Category;

namespace NetCoreMongoDB.Dtos.Book;

public class BookDto : BaseDto
{
    public string BookId { get; set; }
    public string BookName { get; set; }
    public int Price { get; set; }
    public CategoryDto Category { get; set; }
    public string Author { get; set; }
}