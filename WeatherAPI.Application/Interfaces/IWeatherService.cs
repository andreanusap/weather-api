using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.Application.Interfaces;

/// <summary>
/// Weather service interface
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// Get weather summary or list of user's favorite temperatures & locations
    /// </summary>
    /// <param name="unit">The unit of weather in celsius or fahrenheit</param>
    /// <param name="temperature">The temperature</param>
    /// <param name="locations">List of locations in comma separated string</param>
    /// <returns>List of Weather Data View Model</returns>
    Task<IEnumerable<WeatherDataViewModel>> GetWeatherSummary(string unit, double temperature, string locations);

    /// <summary>
    /// Get weather forecast for 5 days by location id
    /// </summary>
    /// <param name="locationId">Location or city id</param>
    /// <returns>Weather Data View Model</returns>
    Task<WeatherDataViewModel> GetWeatherByLocation(string locationId);
}
