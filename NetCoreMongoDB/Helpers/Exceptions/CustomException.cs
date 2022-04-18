using Newtonsoft.Json.Linq;

namespace NetCoreMongoDB.Helpers.Exceptions;

public class CustomException : Exception
{
    public int StatusCode { get; set; }
    public object Result { get; set; }
    public string ContentType { get; set; } = @"text/plain";

    public CustomException(string message, int statusCode, object result = null!)
        : base(message)
    {
        this.StatusCode = statusCode;
        this.Result = result;
    }

    public CustomException(int statusCode, Exception inner)
        : this(inner.ToString(), statusCode) { }

    public CustomException(int statusCode, JObject errorObject)
        : this(errorObject.ToString(), statusCode)
    {
        this.ContentType = @"application/json";
    }
}
