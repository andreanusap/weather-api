using AutoMapper;
using WeatherAPI.Domain.Models;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.Application.Commons;

public class CelsiusWeatherMapper : IWeatherMapper
{
    private readonly IMapper _mapper;

    public CelsiusWeatherMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Map weather data to weather data view model in celsius
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
                Temperature = TemperatureConverters.KelvinToCelcius(x.Main.Temp),
                FeelsLike = TemperatureConverters.KelvinToCelcius(x.Main.FeelsLike),
                MaxTemperature = TemperatureConverters.KelvinToCelcius(x.Main.TempMax),
                MinTemperature = TemperatureConverters.KelvinToCelcius(x.Main.TempMin),
                Humidity = x.Main.Humidity,
            }).ToList(),
        };
    }
}