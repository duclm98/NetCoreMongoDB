using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json;

namespace NetCoreMongoDB.Helpers.Result;

public class CustomResult : ActionResult
{
    private readonly int _httpStatus;
    private readonly string _message;
    private readonly object _result;

    public CustomResult(string message, object result, int httpStatus = 200)
    {
        _httpStatus = httpStatus;
        _message = message;
        _result = result;
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = _httpStatus;

        string resultDetail;
        if (_result is Models.Result result && result.Data is IList data)
        {
            var totalRecord = result.TotalRecord != 0 ? result.TotalRecord : data.Count;
            var pageCount = result.PageCount != 0 ? result.PageCount : data.Count;
            var page = result.Page != 0 ? result.Page : 1;

            resultDetail = new ResultDetail
            {
                Message = _message,
                StatusCode = _httpStatus,
                Result = new ResultResponse
                {
                    TotalRecord = totalRecord,
                    TotalPages = (int)Math.Ceiling(totalRecord / (double)pageCount),
                    PageCount = pageCount,
                    Page = page,
                    TotalInPage = data.Count,
                    Data = data
                }
            }.ToString();
        }
        else
        {
            resultDetail = new ResultDetail()
            {
                Message = _message,
                StatusCode = _httpStatus,
                Result = _result
            }.ToString();
        }

        await context.HttpContext.Response.WriteAsync(resultDetail);
    }
}

public class ResultResponse
{
    public long TotalRecord { get; set; }
    public int TotalPages { get; set; }
    public int PageCount { get; set; }
    public int Page { get; set; }
    public int TotalInPage { get; set; }
    public object Data { get; set; }
}

public class ResultDetail
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public object Result { get; set; } = null!;

    public override string ToString()
    {
        if (Result == null)
            return JsonSerializer.Serialize(new
            {
                statusCode = StatusCode,
                message = Message
            });

        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(new
        {
            statusCode = StatusCode,
            message = Message,
            result = Result
        }, jsonSerializerOptions);
    }
}