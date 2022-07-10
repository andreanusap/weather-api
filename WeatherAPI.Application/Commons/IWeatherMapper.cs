using WeatherAPI.Domain.Models;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.Application.Commons;

/// <summary>
/// Weather mapper interface
/// </summary>
public interface IWeatherMapper
{
    WeatherDataViewModel Map(WeatherData weatherData);
}
