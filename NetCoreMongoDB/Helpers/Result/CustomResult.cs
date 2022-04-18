using Microsoft.AspNetCore.Mvc;

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
        var result = new ResultDetail()
        {
            Message = _message,
            StatusCode = _httpStatus,
            Result = _result
        }.ToString();
        await context.HttpContext.Response.WriteAsync(result);
    }
}