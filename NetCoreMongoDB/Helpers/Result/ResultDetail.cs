using System.Text.Json;

namespace NetCoreMongoDB.Helpers.Result;

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