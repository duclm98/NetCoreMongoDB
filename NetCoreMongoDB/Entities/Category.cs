namespace NetCoreMongoDB.Entities;

public class Category : Base
{
    public string CategoryName { get; set; }
}

public class CategoryLookedUp : Category
{
    public List<Book> Books { get; set; }
}