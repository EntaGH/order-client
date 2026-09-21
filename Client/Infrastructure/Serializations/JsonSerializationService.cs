using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client.Infrastructure.Serializations;

public interface IJsonSerializerService
{
    string Serialize<T>(T obj, Action<JsonSerializerOptions>? configs = null);

    public T? Deserialize<T>(string text);
}

public class JsonSerializerService : IJsonSerializerService
{
    public string Serialize<T>(T obj, Action<JsonSerializerOptions>? configs = null)
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        };
        configs?.Invoke(options);
        return JsonSerializer.Serialize(obj, options);
    }

    public T? Deserialize<T>(string text)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        return JsonSerializer.Deserialize<T>(text, options);
    }
}
