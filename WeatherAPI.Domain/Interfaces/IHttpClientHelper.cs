using WeatherAPI.Domain.Models;

namespace WeatherAPI.Domain.Interfaces;

public interface IHttpClientHelper
{
    /// <summary>
    /// Method helper for Http Get request
    /// </summary>
    /// <param name="uri">The Uri of the request</param>
    /// <returns>Weather Data</returns>
    Task<WeatherData> HttpGet(string uri);
}
