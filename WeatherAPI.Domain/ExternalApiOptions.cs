namespace WeatherAPI.Domain;

/// <summary>
/// Options for the 3rd party API
/// </summary>
public class ExternalApiOptions
{
    public string BaseUrl { get; set; }
    public string ApiKey { get; set; }
}
