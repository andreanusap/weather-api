using System.Text.Json.Serialization;

namespace WeatherAPI.Domain.Models;

public class WeatherData
{
    [JsonPropertyName("cod")]
    public string? Cod { get; set; }

    [JsonPropertyName("message")]
    public int Message { get; set; }

    [JsonPropertyName("cnt")]
    public int Cnt { get; set; }

    [JsonPropertyName("list")]
    public IEnumerable<ListDetail>? List { get; set; }

    [JsonPropertyName("city")]
    public City? City { get; set; }
}
