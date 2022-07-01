using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Runtime.Serialization;
using System.Text.Json;

namespace JobService.SqlServer.Configuration;

public static class Converters
{
    public static ValueConverter<object, string> ObjectAsJsonValueConverter => new ValueConverter<object, string>(
        o => SerializeJson(o),
        v => DeserializeJson(v)
    );

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions();

    private static string SerializeJson(object obj)
    {
        return JsonSerializer.Serialize(obj, _jsonOptions);
    }

    private static object DeserializeJson(string json) =>
        JsonSerializer.Deserialize<object>(json, _jsonOptions) 
        ?? throw new SerializationException($"unable to deserialize value '{json}'");
    
    public static ValueConverter<T, string> EnumValueConverter<T>() 
        where T : Enum => new ValueConverter<T, string>(v => v.ToString(), v => EnumConverter<T>(v));
    public static T EnumConverter<T>(string value) => (T)Enum.Parse(typeof(T), value);
}