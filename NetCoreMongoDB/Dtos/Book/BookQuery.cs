using NetCoreMongoDB.Models;

namespace NetCoreMongoDB.Dtos.Book;

public class BookQuery : Pagination
{
    public string BookName { get; set; }
    public int Price { get; set; }
}