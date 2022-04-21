namespace NetCoreMongoDB.Models;

public class Result
{
    public long TotalRecord { get; set; }
    public object Data { get; set; }
    public int PageCount { get; set; }
    public int Page { get; set; }
}