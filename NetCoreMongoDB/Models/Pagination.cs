namespace NetCoreMongoDB.Models;

public class Pagination
{
    public const int MaxPageCount = 100;

    private int _pageCount = MaxPageCount;
    public int PageCount
    {
        get => _pageCount;
        set => _pageCount = value > MaxPageCount ? MaxPageCount : value;
    }
    public int Page { get; set; } = 1;
}