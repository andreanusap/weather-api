using System.Text.Json.Serialization;

namespace WeatherAPI.Domain.Models;

public class ListDetail
{
    [JsonPropertyName("dt")]
    public long Date { get; set; }

    [JsonPropertyName("main")]
    public MainDetail Main { get; set; }

    [JsonPropertyName("weather")]
    public IEnumerable<Weather>? Weather { get; set; }

    [JsonPropertyName("dt_txt")]
    public string? DateText { get; set; }
}

public class MainDetail
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }

    [JsonPropertyName("humidity")]
    public double Humidity { get; set; }
}

public class Weather
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("main")]
    public string? Main { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}