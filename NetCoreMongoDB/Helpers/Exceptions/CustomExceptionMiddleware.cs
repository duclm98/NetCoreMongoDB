using System.Net;

namespace NetCoreMongoDB.Helpers.Exceptions;

public class CustomExceptionMiddleware
{
    private readonly ILogger logger;
    private readonly RequestDelegate next;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
    {
        this.logger = logger;
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CustomException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception exceptionObj)
        {
            await HandleExceptionAsync(context, exceptionObj);
            logger.LogError(exceptionObj.ToString());
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, CustomException exception)
    {
        string result;
        context.Response.ContentType = "application/json";
        if (exception is CustomException)
        {
            result = new ErrorDetail()
            {
                Message = exception.Message,
                StatusCode = exception.StatusCode,
                Result = exception.Result
            }.ToString();
            context.Response.StatusCode = exception.StatusCode;
        }
        else
        {
            result = new ErrorDetail()
            {
                Message = "Runtime Error",
                StatusCode = (int)HttpStatusCode.BadRequest
            }.ToString();
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        return context.Response.WriteAsync(result);
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        string result = new ErrorDetail()
        {
            Message = exception.Message,
            StatusCode = (int)HttpStatusCode.InternalServerError
        }.ToString();
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }
}