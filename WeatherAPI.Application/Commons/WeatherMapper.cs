using WeatherAPI.Domain.Models;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.Application.Commons;

public class WeatherMapper
{
    private readonly IWeatherMapper _weatherMapper;

    public WeatherMapper(IWeatherMapper weatherMapper)
    {
        _weatherMapper = weatherMapper;
    }

    public WeatherDataViewModel Map(WeatherData weatherData)
    {
        return _weatherMapper.Map(weatherData);
    }
}
