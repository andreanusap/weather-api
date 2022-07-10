using AutoMapper;
using WeatherAPI.Domain.Models;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.Application.Commons;

public class KelvinWeatherMapper : IWeatherMapper
{
    private readonly IMapper _mapper;

    public KelvinWeatherMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Map weather data to weather data view model in kelvin
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
                Temperature = x.Main.Temp,
                FeelsLike = x.Main.FeelsLike,
                Humidity = x.Main.Humidity,
                MaxTemperature = x.Main.TempMax,
                MinTemperature = x.Main.TempMin,
            }).ToList(),
        };
    }
}