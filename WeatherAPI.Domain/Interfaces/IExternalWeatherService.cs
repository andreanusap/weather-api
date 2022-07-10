using WeatherAPI.Domain.Models;

namespace WeatherAPI.Domain.Interfaces;

/// <summary>
/// External weather service interface
/// </summary>
public interface IExternalWeatherService
{
    /// <summary>
    /// Get 5 days forecasts by city id
    /// </summary>
    /// <param name="cityId">City Id</param>
    /// <returns>Weather Data</returns>
    Task<WeatherData> Get5DaysForecastByCityId(string cityId);
}