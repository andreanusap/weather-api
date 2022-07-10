using AutoMapper;
using Microsoft.Extensions.Logging;
using WeatherAPI.Application.Commons;
using WeatherAPI.Application.Interfaces;
using WeatherAPI.Domain.Interfaces;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly IExternalWeatherService _externalWeatherService;
    private readonly IMapper _mapper;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(IExternalWeatherService externalWeatherService, IMapper mapper, ILogger<WeatherService> logger)
    {
        _externalWeatherService = externalWeatherService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Get weather summary or list of user's favorite temperatures & locations
    /// </summary>
    /// <param name="unit">The unit of weather in celsius or fahrenheit</param>
    /// <param name="temperature">The temperature</param>
    /// <param name="locations">List of locations in comma separated string</param>
    /// <returns>List of Weather Data View Model</returns>
    public async Task<WeatherDataViewModel> GetWeatherByLocation(string locationId)
    {
        try
        {
            var weatherData = await _externalWeatherService.Get5DaysForecastByCityId(locationId);
            var mapper = new WeatherMapper(new KelvinWeatherMapper(_mapper));
            return weatherData is null ? null : mapper.Map(weatherData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get weather by location");
            throw ex;
        }
    }

    /// <summary>
    /// Get weather forecast for 5 days by location id
    /// </summary>
    /// <param name="locationId">Location or city id</param>
    /// <returns>Weather Data View Model</returns>
    public async Task<IEnumerable<WeatherDataViewModel>> GetWeatherSummary(string unit, double temperature, string locations)
    {
        try
        {
            var locationIds = locations.Split(',');

            var mapper = new WeatherMapper(new KelvinWeatherMapper(_mapper));
            switch (unit.ToLower())
            {
                case "celsius":
                    temperature = temperature + 273.15;
                    mapper = new WeatherMapper(new CelsiusWeatherMapper(_mapper));
                    break;
                case "fahrenheit":
                    temperature = (temperature - 32) * 5 / 9 + 273.15;
                    mapper = new WeatherMapper(new FahrenheitWeatherMapper(_mapper));
                    break ;
                default:
                    break;
            }

            var todayDate = DateTime.UtcNow;

            var weatherDataList = new List<WeatherDataViewModel>();

            foreach (var locationId in locationIds)
            {
                if (!string.IsNullOrWhiteSpace(locationId.Trim()))
                {
                    var weatherData = await _externalWeatherService.Get5DaysForecastByCityId(locationId.Trim());

                    if (weatherData is not null)
                    {
                        weatherData.List = weatherData.List
                            .Where(x => DateTimeOffset.FromUnixTimeSeconds(x.Date).UtcDateTime.Date == todayDate.AddDays(1).Date
                                        && x.Main.Temp >= temperature)
                            .ToList();

                        weatherDataList.Add(mapper.Map(weatherData));
                    }
                }
            }

            return weatherDataList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get weather summary for favourite locations");
            throw ex;
        }
    }
}
