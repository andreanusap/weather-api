using AutoMapper;
using WeatherAPI.Domain.Models;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.Application.Commons;

public class FahrenheitWeatherMapper : IWeatherMapper
{
    private readonly IMapper _mapper;

    public FahrenheitWeatherMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Map weather data to weather data view model in fahrenheit
    /// </summary>
    /// <param name="weatherData">Weather Data</param>
    /// <returns>Weather Data View Model</returns>
    public WeatherDataViewModel Map(WeatherData weatherData)
    {
        return new WeatherDataViewModel()
        {
            City = _mapper.Map<CityViewModel>(weatherData.City),
            Weathers = weatherData.List.Select(x => new MainWeatherViewModel()
            {
                Date = DateTimeOffset.FromUnixTimeSeconds(x.Date).UtcDateTime,
                DateText = x.DateText,
                WeatherInfos = _mapper.Map<List<WeatherInformationViewModel>>(x.Weather),
                Temperature = Math.Round(1.8 * (x.Main.Temp - 273.15) + 32, 2),
                FeelsLike = Math.Round(1.8 * (x.Main.FeelsLike - 273.15) + 32, 2),
                MaxTemperature = Math.Round(1.8 * (x.Main.TempMax - 273.15) + 32, 2),
                MinTemperature = Math.Round(1.8 * (x.Main.TempMin - 273.15) + 32, 2),
                Humidity = x.Main.Humidity,
            }).ToList(),
        };
    }
}