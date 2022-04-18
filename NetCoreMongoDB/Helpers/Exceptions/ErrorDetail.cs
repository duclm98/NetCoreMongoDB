using System.Text.Json;

namespace NetCoreMongoDB.Helpers.Exceptions;

public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object Result { get; set; }

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